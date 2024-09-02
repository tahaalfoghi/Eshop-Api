using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries
{
    public class GetTransactionsQuery:IRequest<IEnumerable<TransactionDTO>>
    {
    }
}
