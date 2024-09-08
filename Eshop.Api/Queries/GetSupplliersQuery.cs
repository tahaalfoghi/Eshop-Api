using Eshop.DataAccess.Services.Paging;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries
{
    public class GetSupplliersQuery:IRequest<IEnumerable<SupplierDTO>>
    {
        public RequestParameter RequestParameter { get; set; }
        public GetSupplliersQuery(RequestParameter requestParameter)
        {
            RequestParameter = requestParameter;
        }
    }
}
