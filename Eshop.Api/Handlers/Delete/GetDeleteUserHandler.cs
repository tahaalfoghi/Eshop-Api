using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Eshop.Api.Handlers.Delete
{
    public class GetDeleteUserHandler : IRequestHandler<DeleteUserRequest, bool>
    {
        private readonly IUnitOfWork uow;
        private readonly IHttpContextAccessor httpContextAccessor;
        public GetDeleteUserHandler(IUnitOfWork uow, IHttpContextAccessor httpContextAccessor)
        {
            this.uow = uow;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {


            var userId = httpContextAccessor.HttpContext?.User.FindFirstValue("uid");
            if (userId is null)
                throw new NotFoundException($"Invalid id:[{userId}]");

            var user = await uow.UsersRepository.GetUser(userId);
            if (user is null)
                throw new NotFoundException($"user with id:[{userId}] is not found");

            uow.UsersRepository.Delete(user);
            await uow.CommitAsync();

            return true;
        }
    }
}
