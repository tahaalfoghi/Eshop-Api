using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries.Order
{
    public class GetOrderQuery : IRequest<OrderDTO>
    {
        public int OrderId { get; }
        public GetOrderQuery(int OrderId)
        {
            this.OrderId = OrderId;
        }
    }
}
