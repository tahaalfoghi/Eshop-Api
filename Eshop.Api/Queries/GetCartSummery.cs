using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries
{
    public class GetCartSummery:IRequest<IEnumerable<CartDTO>>
    {
    }
}
