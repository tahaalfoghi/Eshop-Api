using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries
{
    public class GetSupplliersQuery:IRequest<IEnumerable<SupplierDTO>>
    {
    }
}
