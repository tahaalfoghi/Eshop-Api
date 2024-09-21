using Eshop.Api.Commands;
using Eshop.DataAccess.Services.ModelService;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class DeleteCartItemHandler:IRequestHandler<DeleteCartItemRequest,bool>
    {
        private readonly ICartService _cartService;

        public DeleteCartItemHandler(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<bool> Handle(DeleteCartItemRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _cartService.DeleteCartItem(request.CartItemId);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
