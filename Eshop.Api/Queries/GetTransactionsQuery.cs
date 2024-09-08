using Eshop.DataAccess.Services.Paging;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries
{
    public class GetTransactionsQuery:IRequest<IEnumerable<TransactionDTO>>
    {
        public RequestParameter requestParameter { get; set; }

        public GetTransactionsQuery(RequestParameter requestParameter)
        {
            this.requestParameter = requestParameter;
        }
    }
}
