using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries.Cart
{
    public class GetCartSummery : IRequest<IEnumerable<CartDTO>>
    {
    }
}
