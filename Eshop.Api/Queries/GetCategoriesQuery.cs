using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries
{
    public class GetCategoriesQuery:IRequest<IEnumerable<CategoryDTO>>
    {
    }
}
