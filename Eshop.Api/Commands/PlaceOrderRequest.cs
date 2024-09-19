using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Commands
{
    public class PlaceOrderRequest:IRequest<bool>
    {
        public OrderPostDTO OrderPostDTO { get; set; }

        public PlaceOrderRequest(OrderPostDTO orderPostDTO)
        {
            OrderPostDTO = orderPostDTO;
        }
    }
}
