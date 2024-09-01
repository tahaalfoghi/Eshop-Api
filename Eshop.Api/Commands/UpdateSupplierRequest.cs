using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Commands
{
    public class UpdateSupplierRequest:IRequest<bool>
    {
        public SupplierDTO Supplier { get; }

        public UpdateSupplierRequest(SupplierDTO supplier)
        {
            Supplier = supplier;
        }
    }
}
