using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries
{
    public class GetProductsQuery:IRequest<IEnumerable<ProductDTO>>
    {
    }
}
