using Eshop.DataAccess.Services.Requests;
using Eshop.Models;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries
{
    public class GetProductsByFilterQuery:IRequest<IEnumerable<ProductDTO>>
    {
        public ProductRequestParamater Param {  get; set; }

        public GetProductsByFilterQuery(ProductRequestParamater param)
        {
            Param = param;
        }
    }
}
