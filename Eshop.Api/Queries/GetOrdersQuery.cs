using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries
{
    public class GetOrdersQuery:IRequest<IEnumerable<OrderDTO>>
    {
    }
}
