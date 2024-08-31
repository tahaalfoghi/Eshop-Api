using eshop.DataAccess.Services.UnitOfWork;
using Eshop.DataAccess.Services.Auth;
using Eshop.Models;
using Eshop.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController:ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IAuthService authService;
        public UsersController(IUnitOfWork uow, IAuthService authService)
        {
            this.uow = uow;
            this.authService = authService;
        }
        [HttpGet]
        [Route("Users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await uow.UsersRepository.GetUsersRole();
            if (users is null)
                return BadRequest("Users not found");

            return Ok(users);
        }
        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest($"Invalid input request {ModelState}");
            
            var result = await authService.RegisterAsync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok($"User {{{model.UserName}, {model.Email}}} created successfully");
        }
    }
}
