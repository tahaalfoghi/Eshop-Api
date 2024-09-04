using Eshop.Models;
using Eshop.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly JWT jwt;
        private readonly RoleManager<IdentityRole> roleManager;

        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.jwt = jwt.Value;
            this.roleManager = roleManager;
        }

        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            if (await userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthModel { Message = "Email is already register !" };

            if (await userManager.FindByEmailAsync(model.UserName) is not null)
                return new AuthModel { Message = "UserName is already register !" };

            var user = new ApplicationUser
            {
                FirstName = model.UserName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                Phone = model.Phone
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var e in result.Errors)
                {
                    errors += $"{e.Description}, ";
                }
                return new AuthModel { Message = errors };
            }

            await userManager.AddToRoleAsync(user, "Admin");

            var jwtSecurityToken = await CreateJwtTokenAsync(user);

            return new AuthModel
            {
                Email = user.Email,
                ExpirsOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "Customer" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = user.UserName,
            };
        }
        public async Task<JwtSecurityToken> CreateJwtTokenAsync(ApplicationUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(jwt.Duration),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<AuthModel> LoginAsync(TokenRequestModel request)
        {
            var authModel = new AuthModel();
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
            {
                authModel.Message = $"Email or Password is incorrect";
                return authModel;
            }

            var jwtSecurityToken = await CreateJwtTokenAsync(user);
            var roles = await userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.UserName = user.UserName;
            authModel.Roles = roles.ToList();


            return authModel;
        }

        public async Task<string> AddRoleAsync(AddModelRole model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user is null || !await roleManager.RoleExistsAsync(model.RoleName))
                return "Invalid User Id or Role Name";

            if (await userManager.IsInRoleAsync(user, model.RoleName))
                return "User already assigned to this role";

            var result = await userManager.AddToRoleAsync(user,model.RoleName);

            return result.Succeeded ? string.Empty : "Something went wrong";

        }
    }
}
