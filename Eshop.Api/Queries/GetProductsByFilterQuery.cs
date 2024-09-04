using Eshop.Models;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries
{
    public class GetProductsByFilterQuery:IRequest<IEnumerable<ProductDTO>>
    {
        public TableSearch search;

        public GetProductsByFilterQuery(TableSearch search)
        {
            this.search = search;
        }
    }
}
