using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries.Order;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Handlers.GetById
{
    public class GetOrderHandler : IRequestHandler<GetOrderQuery, OrderDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetOrderHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<OrderDTO> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            if (request.OrderId <= 0)
                throw new BadRequestException($"Invalid id:{request.OrderId}");

            var order = await uow.OrderRepository.GetByIdAsync(request.OrderId, includes: "ApplicationUser,OrderDetails");
            if (order is null)
                throw new NotFoundException($"No orders exists in database");

            return mapper.Map<OrderDTO>(order);
        }

    }
}
