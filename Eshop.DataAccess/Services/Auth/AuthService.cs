using Eshop.Models;
using Eshop.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
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
                return new AuthModel { Message = "Email is already registered !" };

            if (await userManager.FindByEmailAsync(model.UserName) is not null)
                return new AuthModel { Message = "UserName is already registered !" };

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

            await userManager.AddToRoleAsync(user, "Customer");

            var jwtSecurityToken = await CreateJwtTokenAsync(user);

            return new AuthModel
            {
                Email = user.Email,
                //ExpirsOn = jwtSecurityToken.ValidTo,
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
            //authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = roles.ToList();
            if (user.RefreshTokens.Any(x => x.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(x => x.IsActive);
                authModel.RefreshToken = activeRefreshToken.Token;
                authModel.RefreshTokenExpiration = activeRefreshToken.ExpireOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                authModel.RefreshToken = refreshToken.Token;
                authModel.RefreshTokenExpiration = refreshToken.ExpireOn;
                user.RefreshTokens.Add(refreshToken);
                await userManager.UpdateAsync(user);
            }

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
        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpireOn = DateTime.UtcNow.AddDays(2),
                CreatedAt = DateTime.UtcNow,
            };
        }

        public async Task<AuthModel> GenerateRefreshToken(string refreshToken)
        {
            var authModel = new AuthModel();
            
            var user = await userManager.Users.SingleOrDefaultAsync(x => x.RefreshTokens.Any(x=>x.Token == refreshToken));
            if(user is null)
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "Invalid Token";
                return authModel;
            }
            var token = user.RefreshTokens.Single(x=>x.Token == refreshToken);
            if (!token.IsActive)
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "InActive Token";
                return authModel;
            }

            token.RevokedOn = DateTime.UtcNow;
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await userManager.UpdateAsync(user);

            var jwtToken = await CreateJwtTokenAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authModel.Email = user.Email;
            authModel.UserName = user.UserName;
            var roles =  await userManager.GetRolesAsync(user);
            authModel.RefreshToken = newRefreshToken.Token;
            authModel.RefreshTokenExpiration = newRefreshToken.ExpireOn;

            return authModel;
        }

        public async Task<bool> ValidateUser(TokenRequestModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            return model is not null && await userManager.CheckPasswordAsync(user, model.Password);
        }

        public async Task Logout()
        {
          
        }
    }
}
