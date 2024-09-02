using Eshop.Models;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries
{
    public class GetSupplierByFilterQuery:IRequest<SupplierDTO>
    {
        public TableSearch Search { get; }

        public GetSupplierByFilterQuery(TableSearch Search)
        {
            this.Search = Search;
        }
    }
}
