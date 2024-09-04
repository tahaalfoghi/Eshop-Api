using Eshop.Models.DTOModels;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Eshop.Api.Commands
{
    public class UpdatePatchProductRequest:IRequest<bool>
    {
        public int productId {  get; set; }
        public JsonPatchDocument<ProductPostDTO> patch { get; set; }

        public UpdatePatchProductRequest(int productId,JsonPatchDocument<ProductPostDTO> patch)
        {
            this.productId = productId;
            this.patch = patch;
        }
    }
}
