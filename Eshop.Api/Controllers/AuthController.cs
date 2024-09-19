using Eshop.DataAccess.Services.Auth;
using Eshop.DataAccess.Services.Validators;
using Eshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
namespace Eshop.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Eshop-UI")]

    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly ILogger<AuthController> logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            this.authService = authService;
            this.logger = logger;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromForm]RegisterModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest($"Invalid model {ModelState}");

            var validate = new RegisterModelValidator();
            var check = validate.Validate(model);
            if (!check.IsValid)
                return BadRequest(check.Errors.ToString());

            var result = await authService.RegisterAsync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            logger.LogInformation($"New user regitered to the system:[ {model.UserName}, {model.Email} ]");
            return Ok(new {Message = "Registration successful!" });
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromForm] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest($"Invalid model {ModelState}");

            var result = await authService.LoginAsync(model);
            if (!result.IsAuthenticated)
            {
                logger.LogError($"Error while attempt to login user:[ {model.Email} ] is not authenticated");
                return BadRequest(result.Message);
            }
            if (!string.IsNullOrEmpty(result.RefreshToken))
            {
                SetRefreshTokenInCookie(result.RefreshToken,result.RefreshTokenExpiration);
            }
            logger.LogInformation($"user [ {model.Email} ] login to the system");
            return Ok(new {Message= "You Logged in successfully", token = result.Token});
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        [Route("AddRole")]
        public async Task<IActionResult> AddRoleToUser([FromForm] AddModelRole model)
        {
            if (!ModelState.IsValid)
                return BadRequest($"Invalid model {ModelState}");

            var result = await authService.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok($"User assigned to [{model.RoleName}] role successfully");
        }
        [HttpGet]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await authService.GenerateRefreshToken(refreshToken);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result);
            }

            SetRefreshTokenInCookie(result.RefreshToken,result.RefreshTokenExpiration);

            return Ok(result);
        }
        private void SetRefreshTokenInCookie(string refreshToken,DateTime expires)
        {
            var cookieOption = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime()
            };
            Response.Cookies.Append("refreshToken",refreshToken,cookieOption);
        }
    }
}
