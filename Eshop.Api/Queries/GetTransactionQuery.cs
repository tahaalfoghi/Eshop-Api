using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries
{
    public class GetTransactionQuery:IRequest<TransactionDTO>
    {
        public int TransactionId { get; }
        public GetTransactionQuery(int TransactionId)
        {
            this.TransactionId = TransactionId;
        }
    }
}
