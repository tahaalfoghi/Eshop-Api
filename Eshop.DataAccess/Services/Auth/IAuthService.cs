using Eshop.Models;
using Eshop.Models.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> LoginAsync(TokenRequestModel request);
        Task<JwtSecurityToken> CreateJwtTokenAsync(ApplicationUser user);
        Task<string> AddRoleAsync(AddModelRole model);
        Task<AuthModel> GenerateRefreshToken(string refreshToken);
    }
}
