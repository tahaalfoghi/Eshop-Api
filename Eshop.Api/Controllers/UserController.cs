using eshop.DataAccess.Services.UnitOfWork;
using Eshop.DataAccess.Services.Auth;
using Eshop.Models;
using Eshop.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Eshop-UI")]
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
        [HttpGet]
        [Route("Users/{userId:Guid}")]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            if (string.IsNullOrEmpty(userId.ToString()))
                return BadRequest($"Invalid id: {userId.ToString()}");

            var user = await uow.UsersRepository.GetUser(userId.ToString());
            if (user is null)
                return NotFound($"user not found");

            return Ok(user);
        }
        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser([FromForm] RegisterModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest($"Invalid input request {ModelState}");
            
            var result = await authService.RegisterAsync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok($"User {{{model.UserName}, {model.Email}}} created successfully");
        }
        [HttpDelete]
        [Route("DeleteUser/{userId:Guid}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            if (string.IsNullOrEmpty(userId.ToString()))
                return BadRequest($"Invalid id");

            var user = await uow.UsersRepository.GetUser(userId.ToString());
            if (user is null)
                return NotFound($"user not found");

            uow.UsersRepository.Delete(user);
            await uow.CommitAsync();

            return Ok($"User: {user.UserName} deleted successfully");
        }
        [HttpDelete]
        [Route("DeleteUser/{userName}")]
        public async Task<IActionResult> DeleteUserbyUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return BadRequest($"Invalid id");

            var user = await uow.UsersRepository.GetUserByUserName(userName);
            if (user is null)
                return NotFound($"user not found");

            uow.UsersRepository.Delete(user);
            await uow.CommitAsync();

            return Ok($"User: {user.UserName} deleted successfully");
        }
    }
}
