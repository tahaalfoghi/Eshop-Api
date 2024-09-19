using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Commands
{
    public class ChangeOrderStatusRequest:IRequest<bool>
    {
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; }

        public ChangeOrderStatusRequest(int orderId, OrderStatus status)
        {
            OrderId = orderId;
            Status = status;
        }
    }
    
}
