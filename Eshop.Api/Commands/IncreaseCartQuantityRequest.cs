using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Commands
{
    public class IncreaseCartQuantityRequest:IRequest<bool>
    {
        public int cartItemId { get; set; }
        public int ProductId { get; set; }
        public CartPostDTO CartPostDTO { get; set; }

        public IncreaseCartQuantityRequest(int cartItemId, int productId, CartPostDTO cartPostDTO)
        {
            this.cartItemId = cartItemId;
            ProductId = productId;
            CartPostDTO = cartPostDTO;
        }
    }
}
