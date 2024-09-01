using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries
{
    public class GetSuppllierQuery : IRequest<SupplierDTO>
    {
        public int SupplierId { get; }
        public GetSuppllierQuery(int SupplierId)
        {
            this.SupplierId = SupplierId;
        }
    }
}
