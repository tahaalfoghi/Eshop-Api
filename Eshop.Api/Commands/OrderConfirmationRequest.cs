using MediatR;

namespace Eshop.Api.Commands
{
    public class OrderConfirmationRequest:IRequest<bool>
    {
        public int OrderId { get; set; }

        public OrderConfirmationRequest(int orderId)
        {
            OrderId = orderId;
        }
    }
}
