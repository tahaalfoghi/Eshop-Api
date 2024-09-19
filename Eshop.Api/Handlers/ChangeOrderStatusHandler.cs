using Eshop.Api.Commands;
using Eshop.DataAccess.Services.Middleware;
using Eshop.DataAccess.Services.ModelService;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class ChangeOrderStatusHandler : IRequestHandler<ChangeOrderStatusRequest, bool>
    {
        private readonly IOrderService _orderService;

        public ChangeOrderStatusHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<bool> Handle(ChangeOrderStatusRequest request, CancellationToken cancellationToken)
        {
            if (request.OrderId <= 0)
                throw new BadRequestException("Invalid id");

            await _orderService.ChangeOrderStatus(request.OrderId, request.Status);
            return true;
        }
    }
}
