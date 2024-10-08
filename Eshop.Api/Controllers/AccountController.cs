﻿using Eshop.Api.Commands;
using Eshop.Api.Commands.Delete;
using Eshop.Api.Commands.Update;
using Eshop.Api.Queries;
using Eshop.Api.Queries.User;
using Eshop.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eshop.Api.Controllers
{
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Eshop-UI")]
    public class AccountController:ControllerBase
    {
        private readonly IMediator mediator;

        public AccountController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        [Route("Profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = HttpContext.User.FindFirstValue("uid");
            var query = new GetUserQuery(userId);
            var result = await mediator.Send(query);

            return Ok(result);
        }
        [HttpPut]
        [Route("EditProfile")]
        public async Task<IActionResult> EditProfile([FromBody] UserModelDTO model)
        {
            var command = new UpdateUserRequest(model);
            var result = await mediator.Send(command);
            return result ? Ok($"Your Profile has been updated successfully") : BadRequest();
        }
        
    }
}
