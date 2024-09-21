using Eshop.Api.Commands;
using Eshop.DataAccess.Services.ModelService;
using MediatR;
using NuGet.Protocol.Plugins;

namespace Eshop.Api.Handlers
{
    public class AddToCartHandler:IRequestHandler<AddToCartRequest,bool>
    {
        private readonly ICartService _cartService;

        public AddToCartHandler(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<bool> Handle(AddToCartRequest request, CancellationToken cancellationToken)
        {
            try
            {

                await _cartService.AddToCart(request.CartPostDTO);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            } 

        }
    }
}
