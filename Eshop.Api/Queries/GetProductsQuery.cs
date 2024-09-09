using Eshop.DataAccess.Services.Requests;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries
{
    public class GetProductsQuery:IRequest<IEnumerable<ProductDTO>>
    {
        public RequestParameter requestParameter { get; set; }

        public GetProductsQuery(RequestParameter requestParameter)
        {
            this.requestParameter = requestParameter;
        }
    }
}
