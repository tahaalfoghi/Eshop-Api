using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class GetUserHandler:IRequestHandler<GetUserQuery,UserModelDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetUserHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }
        public async Task<UserModelDTO> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.userId))
                throw new BadRequestException($"userId is invalid");

            var user = await uow.UsersRepository.GetUser(request.userId);
            if (user is null)
                throw new NotFoundException($"user not found");

            return mapper.Map<UserModelDTO>(user);
        }
    }
}
