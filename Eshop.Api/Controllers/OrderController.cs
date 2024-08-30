using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Models.DTOModels;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController:ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        public OrderController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }
        [HttpGet]
        [Route("GetOrders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await uow.OrderRepository.GetAllAsync(includes:"ApplicationUser");
            if (orders is null)
                return BadRequest($"No orders found");

            var dto_orders = mapper.Map<List<OrderDTO>>(orders);
            return Ok(dto_orders);
        }
        [HttpGet]
        [Route("GetOrder/{Id:int}")]
        public async Task<IActionResult> GetOrder([FromRoute] int Id)
        {
            if (Id <= 0)
                return BadRequest($"Invalid id value:{Id}");

            var order = await uow.OrderRepository.GetByIdAsync(Id,includes:"Product,ApplicationUser");
            if (order is null)
                return BadRequest($"Order {Id} not found");

            return Ok(order);
        }
        [HttpPost]
        [Route("OrderConfirmation/{orderId:int}")]
        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            if (orderId <= 0)
                return BadRequest($"Invalid id:{orderId}");

            var order = await uow.OrderRepository.GetByIdAsync(orderId, includes:"ApplicationUser");
            if (order is null)
                return BadRequest($"Order not found");

            order.Status = OrderStatus.Pending.ToString();
            var orderCarts = await uow.CartRepository.GetCartsAsync(x=>x.UserId == order.ApplicationUser.Id);
            uow.CartRepository.DeleteRangeAsync(orderCarts);
            await uow.CommitAsync();

            return Ok($"order successfully confirmed");

        }
    }
}
