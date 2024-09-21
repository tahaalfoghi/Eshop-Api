using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Commands
{
    public class AddToCartRequest:IRequest<bool>
    {
        public CartPostDTO CartPostDTO { get; set; }

        public AddToCartRequest(CartPostDTO cartPostDTO)
        {
            CartPostDTO = cartPostDTO;
        }
    }
}
