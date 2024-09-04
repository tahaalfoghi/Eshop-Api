using Eshop.Models;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries
{
    public class GetProductByFilterQuery:IRequest<ProductDTO>
    {
        public TableSearch search;

        public GetProductByFilterQuery(TableSearch search)
        {
            this.search = search;
        }
    }
}
