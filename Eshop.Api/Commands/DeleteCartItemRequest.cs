using MediatR;

namespace Eshop.Api.Commands
{
    public class DeleteCartItemRequest:IRequest<bool>
    {
        public int CartItemId { get; set; }

        public DeleteCartItemRequest(int cartItemId)
        {
            CartItemId = cartItemId;
        }
    }
}
