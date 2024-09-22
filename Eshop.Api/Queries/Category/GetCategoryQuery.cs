using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries.Category
{
    public class GetCategoryQuery : IRequest<CategoryDTO>
    {
        public int CategoryId { get; }

        public GetCategoryQuery(int categoryId)
        {
            CategoryId = categoryId;
        }
    }
}
