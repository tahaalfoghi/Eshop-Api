using Eshop.Models;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries
{
    public class GetSuppliersByFilterQuery:IRequest<IEnumerable<SupplierDTO>>
    {
        public TableSearch Search { get;}

        public GetSuppliersByFilterQuery(TableSearch Search)
        {
            this.Search = Search;
        }
    }
}
