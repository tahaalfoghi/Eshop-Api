﻿using Eshop.DataAccess.Services.Auth;
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

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest($"Invalid model {ModelState}");

            var result = await authService.RegisterAsync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            //return Ok(new {token = result.Token, ExpiresOn = result.ExpirsOn});
            return Ok(result);
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest($"Invalid model {ModelState}");

            var result = await authService.LoginAsync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(new {token = result.Token, ExpiresOn = result.ExpirsOn});
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        [Route("AddRole")]
        public async Task<IActionResult> AddRoleToUser([FromBody] AddModelRole model)
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
