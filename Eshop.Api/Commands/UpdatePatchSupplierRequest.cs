using Eshop.Models.DTOModels;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Eshop.Api.Commands
{
    public class UpdatePatchSupplierRequest:IRequest<bool>
    {
        public int supplierId { get; set; }
        public JsonPatchDocument<SupplierDTO> patch { get; set; }

        public UpdatePatchSupplierRequest(int supplierId, JsonPatchDocument<SupplierDTO> patch)
        {
            this.supplierId = supplierId;
            this.patch = patch;
        }
    }
}
