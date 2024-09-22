using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries.Transaction;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;
using Newtonsoft.Json;

namespace Eshop.Api.Handlers.Get
{
    public class GetTransactionsHandler : IRequestHandler<GetTransactionsQuery, IEnumerable<TransactionDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        public GetTransactionsHandler(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<TransactionDTO>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            var transactions = await uow.TransactionRepository.GetAllAsync(request.requestParameter, includes: "ApplicationUser");
            if (transactions is null || transactions.Count() == 0)
                throw new NotFoundException("No transaction exists in database");

            httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(transactions.MetaData));
            return mapper.Map<IEnumerable<TransactionDTO>>(transactions);
        }
    }
}
