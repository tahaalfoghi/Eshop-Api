using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries.Transaction
{
    public class GetPaymentQuery : IRequest<PaymentDTO>
    {
        public int TransactionId { get; }
        public GetPaymentQuery(int TransactionId)
        {
            this.TransactionId = TransactionId;
        }
    }
}
