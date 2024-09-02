using Eshop.Models;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries
{
    public class GetCategoriesByFilterQuery:IRequest<IEnumerable<CategoryDTO>>
    {
        public TableSearch Search { get; }

        public GetCategoriesByFilterQuery(TableSearch Search)
        {
            this.Search = Search;
        }
    }
}
