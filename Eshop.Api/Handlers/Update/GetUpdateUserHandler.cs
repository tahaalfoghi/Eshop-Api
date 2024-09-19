using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands.Update;
using Eshop.DataAccess.Services.Middleware;
using Eshop.DataAccess.Services.Validators;
using Eshop.Models.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NuGet.Protocol.Plugins;
using System.Security.Claims;

namespace Eshop.Api.Handlers.Update
{
    public class GetUpdateUserHandler : IRequestHandler<UpdateUserRequest, bool>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;

        public GetUpdateUserHandler(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public async Task<bool> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var validate = new UserModelDTOValidator();
            var result = validate.Validate(request.userDto);
            if (!result.IsValid)
                throw new BadRequestException($"{string.Join(",", result.Errors)}");

            var userId = httpContextAccessor.HttpContext.User.FindFirstValue("uid");
            var SignedInUser = await uow.UsersRepository.GetUser(userId);

            SignedInUser.FirstName = request.userDto.FirstName;
            SignedInUser.LastName = request.userDto.LastName;
            SignedInUser.Email = request.userDto.Email;
            SignedInUser.UserName = request.userDto.UserName;
            SignedInUser.Phone = request.userDto.Phone;

            //await uow.UsersRepository.UpdateUserAsync(SignedInUser);
            await uow.CommitAsync();

            return true;
        }
    }
}
