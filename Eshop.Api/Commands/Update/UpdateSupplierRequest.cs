using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Commands.Update
{
    public class UpdateSupplierRequest : IRequest<bool>
    {
        public int SupplierId { get; set; }
        public SupplierDTO Supplier { get; }

        public UpdateSupplierRequest(int supplierId, SupplierDTO supplier)
        {
            SupplierId = supplierId;
            Supplier = supplier;
        }
    }
}
