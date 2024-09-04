using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Commands
{
    public class UpdateProductRequest:IRequest<bool>
    {
        public int productId { get; set; }
        public ProductPostDTO productPostDTO { get; set; }

        public UpdateProductRequest(int productId, ProductPostDTO productPostDTO)
        {
            this.productId = productId;
            this.productPostDTO = productPostDTO;
        }
    }
}
