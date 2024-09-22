using Eshop.DataAccess.Services.Requests;
using Eshop.Models;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries.Category
{
    public class GetCategoriesByFilterQuery : IRequest<IEnumerable<CategoryDTO>>
    {
        public CategoryRequestParamater Params { get; set; }
        public GetCategoriesByFilterQuery(CategoryRequestParamater Params)
        {
            this.Params = Params;
        }

    }
}
