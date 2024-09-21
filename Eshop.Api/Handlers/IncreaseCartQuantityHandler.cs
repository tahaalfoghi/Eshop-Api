using Eshop.Api.Commands;
using Eshop.DataAccess.Services.ModelService;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class IncreaseCartQuantityHandler:IRequestHandler<IncreaseCartQuantityRequest,bool>
    {
        private readonly ICartService _cartService;

        public IncreaseCartQuantityHandler(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<bool> Handle(IncreaseCartQuantityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _cartService.IncreaseCartQuantity(request.cartItemId,request.ProductId,request.CartPostDTO);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
    public class DecreaseCartQuantityHandler : IRequestHandler<DecreaseCartQuantityRequest, bool>
    {
        private readonly ICartService _cartService;

        public DecreaseCartQuantityHandler(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<bool> Handle(DecreaseCartQuantityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _cartService.DecreaseCartQuantity(request.cartItemId, request.ProductId, request.CartPostDTO);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
