using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Commands.Update
{
    public class UpdateTransacrtionRequest : IRequest<bool>
    {
        public PaymentDTO TransactionDto { get; }
        public UpdateTransacrtionRequest(PaymentDTO TransactionDto)
        {
            this.TransactionDto = TransactionDto;
        }
    }
}
