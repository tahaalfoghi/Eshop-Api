using Eshop.DataAccess.Services.Auth;
using Eshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Eshop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly ILogger<AuthController> logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            this.authService = authService;
            this.logger = logger;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromForm]RegisterModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest($"Invalid model {ModelState}");

            var result = await authService.RegisterAsync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            logger.LogInformation($"New user regitered to the system:[ {model.UserName}, {model.Email} ]");
            return Ok(new {token = result.Token, ExpiresOn = result.ExpirsOn});
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

            logger.LogInformation($"user [ {model.Email} ] login to the system");
            return Ok(new {token = result.Token, ExpiresOn = result.ExpirsOn});
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

            return Ok($"User assigned to role successfully");
        }
    }
}
