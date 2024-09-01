using MediatR;

namespace Eshop.Api.Commands
{
    public class DeleteSupplierRequest:IRequest<bool>
    {
        public int SupplierId { get;}

        public DeleteSupplierRequest(int supplierId)
        {
            SupplierId = supplierId;
        }
    }
}
