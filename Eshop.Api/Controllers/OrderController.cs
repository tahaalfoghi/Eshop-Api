using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace Eshop.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController:ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IMediator mediator;
        public OrderController(IUnitOfWork uow, IMapper mapper, IMediator mediator)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.mediator = mediator;
        }
        [HttpGet]
        [Route("Orders")]
        public async Task<IActionResult> GetOrders()
        {
            var query = new GetOrdersQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpGet]
        [Route("Orders/{orderId:int}")]
        public async Task<IActionResult> GetOrder([FromRoute] int orderId)
        {
            var query = new GetOrderQuery(orderId);
            var result = await mediator.Send(query);
            return Ok(result);
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
            var transaction = new Models.Models.Transaction()
            {
                OrderId = order.Id,
                UserId = order.ApplicationUser.Id,
                Amount = order.TotalPrice,
                CreatedAt = DateTime.UtcNow,
            };
            await uow.TransactionRepository.CreateAsync(transaction);
            var orderCarts = await uow.CartRepository.GetCartsAsync(x=>x.UserId == order.ApplicationUser.Id);
            uow.CartRepository.DeleteRangeAsync(orderCarts);
            await uow.CommitAsync();

            return Ok($"order successfully confirmed");

        }
        [HttpPost]
        [Route("ChangeOrderStatus/{orderId:int}")]
        public async Task<IActionResult> ChangeOrderStatus(int orderId,OrderStatus status)
        {
            if (orderId <= 0)
                return BadRequest($"Invalid Id value:{orderId}");

            if (string.IsNullOrEmpty(status.ToString()))
                return BadRequest($"Order status value is null");

            var order = await uow.OrderRepository.GetByIdAsync(orderId);
            if (order is null)
                return BadRequest($"Order with id:{orderId} is not found");

            var oldStatus = order.Status;
            uow.OrderRepository.ChangeStatus(order, status);
            await uow.CommitAsync();

            if (order.Status.Equals("Canceled"))
            {
                var trans = await uow.TransactionRepository.GetAsync(x=>x.OrderId == order.Id);
                uow.TransactionRepository.Delete(trans);
            }

            return Ok($"Order:{order.Id} status changed from:`{oldStatus}` to `{status.ToString()}`");
        }
    }
}
