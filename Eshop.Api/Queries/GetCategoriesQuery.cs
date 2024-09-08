using Eshop.DataAccess.Services.Paging;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries
{
    public class GetCategoriesQuery:IRequest<IEnumerable<CategoryDTO>>
    {
        public RequestParameter requestParameter { get; set; }

        public GetCategoriesQuery(RequestParameter requestParameter)
        {
            this.requestParameter = requestParameter;
        }
    }
}
