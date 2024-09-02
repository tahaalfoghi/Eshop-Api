using MediatR;

namespace Eshop.Api.Commands
{
    public class DeleteTransactionRequest:IRequest<bool>
    {
        public int TransactionId { get; }
        public DeleteTransactionRequest(int TransactionId)
        {
            this.TransactionId = TransactionId; 
        }
    }
}
