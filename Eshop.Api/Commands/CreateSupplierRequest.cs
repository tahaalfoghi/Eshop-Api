using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using MediatR;

namespace Eshop.Api.Commands
{
    public class CreateSupplierRequest:IRequest<SupplierDTO>
    {
        public SupplierDTO Supplier { get; }

        public CreateSupplierRequest(SupplierDTO Supplier)
        {
            this.Supplier = Supplier;
        }

    }
}
