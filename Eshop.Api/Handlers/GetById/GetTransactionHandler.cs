using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Handlers.GetById
{
    public class GetTransactionHandler : IRequestHandler<GetTransactionQuery, TransactionDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetTransactionHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<TransactionDTO> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
        {
            var transaction = await uow.TransactionRepository.GetByIdAsync(request.TransactionId, "ApplicationUser");
            if (transaction is null)
                throw new NotFoundException();

            return mapper.Map<TransactionDTO>(transaction);
        }
    }
}
