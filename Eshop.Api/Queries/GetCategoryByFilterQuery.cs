using Eshop.Models;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries
{
    public class GetCategoryByFilterQuery:IRequest<CategoryDTO>
    {
        public TableSearch Search { get; }

        public GetCategoryByFilterQuery(TableSearch Search)
        {
            this.Search = Search;
        }
    }
}
