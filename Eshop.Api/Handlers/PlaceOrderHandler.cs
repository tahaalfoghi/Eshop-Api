using Eshop.Api.Commands;
using Eshop.DataAccess.Services.ModelService;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class PlaceOrderHandler:IRequestHandler<PlaceOrderRequest,bool>
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<PlaceOrderHandler> _logger;
        public PlaceOrderHandler(IOrderService orderService, ILogger<PlaceOrderHandler> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        public async Task<bool> Handle(PlaceOrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Place order request");
                await _orderService.PlaceOrder(request.OrderPostDTO);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred in the place order request cqrs");
                throw new Exception(ex.Message);
            }
        }
    }
}
