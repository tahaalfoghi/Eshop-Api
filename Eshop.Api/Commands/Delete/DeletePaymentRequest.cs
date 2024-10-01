using MediatR;

namespace Eshop.Api.Commands.Delete
{
    public class DeletePaymentRequest : IRequest<bool>
    {
        public int TransactionId { get; }
        public DeletePaymentRequest(int TransactionId)
        {
            this.TransactionId = TransactionId;
        }
    }
}
