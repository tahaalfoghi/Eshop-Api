using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Commands.Update
{
    public class UpdatePaymentRequest : IRequest<bool>
    {
        public PaymentDTO TransactionDto { get; }
        public UpdatePaymentRequest(PaymentDTO TransactionDto)
        {
            this.TransactionDto = TransactionDto;
        }
    }
}
