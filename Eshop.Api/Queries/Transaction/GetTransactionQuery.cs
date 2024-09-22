using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries.Transaction
{
    public class GetTransactionQuery : IRequest<TransactionDTO>
    {
        public int TransactionId { get; }
        public GetTransactionQuery(int TransactionId)
        {
            this.TransactionId = TransactionId;
        }
    }
}
