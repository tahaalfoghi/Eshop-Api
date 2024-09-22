using Eshop.Api.Queries.Cart;
using Eshop.DataAccess.Services.ModelService;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class CartSummeryHandler : IRequestHandler<GetCartSummery, IEnumerable<CartDTO>>
    {
        private readonly ICartService _cartService;

        public CartSummeryHandler(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IEnumerable<CartDTO>> Handle(GetCartSummery request, CancellationToken cancellationToken)
        {
            return await _cartService.Summery();
        }
    }
}
