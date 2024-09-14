using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;
using Newtonsoft.Json;

namespace Eshop.Api.Handlers.Get
{
    public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, IEnumerable<OrderDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        public GetOrdersHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<OrderDTO>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await uow.OrderRepository.GetAllAsync(request.requestParameter, includes: "ApplicationUser,OrderDetails");
            if (orders is null)
                throw new NotFoundException($"No orders exists in database");

            httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(orders.MetaData));
            return mapper.Map<IEnumerable<OrderDTO>>(orders);
        }
    }
}
