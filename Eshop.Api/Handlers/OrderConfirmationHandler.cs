using Eshop.Api.Commands;
using Eshop.DataAccess.Services.ModelService;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class OrderConfirmationHandler:IRequestHandler<OrderConfirmationRequest,bool>
    {
        private readonly IOrderService _orderService;

        public OrderConfirmationHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<bool> Handle(OrderConfirmationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _orderService.OrderConfirmation(request.OrderId);
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message,ex.InnerException);
            }
            
        }
    }
}
