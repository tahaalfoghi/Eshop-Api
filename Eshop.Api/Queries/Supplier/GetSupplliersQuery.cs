using Eshop.DataAccess.Services.Requests;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries.Supplier
{
    public class GetSupplliersQuery : IRequest<IEnumerable<SupplierDTO>>
    {
        public RequestParameter RequestParameter { get; set; }
        public GetSupplliersQuery(RequestParameter requestParameter)
        {
            RequestParameter = requestParameter;
        }
    }
}
