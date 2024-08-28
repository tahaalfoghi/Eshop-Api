using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
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
            var orders = await uow.OrderRepository.GetAllAsync(includes:"Product,ApplicationUser");
            if (orders is null)
                return BadRequest($"No orders found");

            return Ok(orders);
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
    }
}
