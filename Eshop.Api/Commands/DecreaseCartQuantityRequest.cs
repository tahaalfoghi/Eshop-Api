using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Commands
{
    public class DecreaseCartQuantityRequest:IRequest<bool>
    {
        public int cartItemId { get; set; }
        public int ProductId { get; set; }
        public CartPostDTO CartPostDTO { get; set; }

        public DecreaseCartQuantityRequest(int cartItemId, int productId, CartPostDTO cartPostDTO)
        {
            this.cartItemId = cartItemId;
            ProductId = productId;
            CartPostDTO = cartPostDTO;
        }
    }
}
