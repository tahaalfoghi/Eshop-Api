using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries
{
    public class GetUserCartQuery:IRequest<IEnumerable<CartDTO>>
    {
    }
}
