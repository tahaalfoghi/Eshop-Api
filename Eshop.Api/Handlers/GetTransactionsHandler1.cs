using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class GetTransactionsHandler:IRequestHandler<GetTransactionsQuery,IEnumerable<TransactionDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetTransactionsHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TransactionDTO>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            var transactions = await uow.TransactionRepository.GetAllAsync(request.requestParameter, includes:"ApplicationUser");
            if (transactions is null || transactions.Count() == 0)
                throw new NotFoundException("No transaction exists in database");

            return mapper.Map<IEnumerable<TransactionDTO>>(transactions);
        }
    }
}
