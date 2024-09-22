using AutoMapper;
using eshop.DataAccess.Data;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands;
using Eshop.Api.Queries;
using Eshop.Api.Queries.Order;
using Eshop.DataAccess.Services.Requests;
using Eshop.Models.DTOModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers
{
    
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Eshop-UI")]

    public class OrderController:ControllerBase
    {
        private readonly IMediator mediator;
        public OrderController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("Orders")]
        public async Task<IActionResult> GetOrders([FromQuery] RequestParameter requestParameter)
        {
            var query = new GetOrdersQuery(requestParameter);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
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
            var command = new OrderConfirmationRequest(orderId);
            var result = await mediator.Send(command);
            return result ? Ok($"order successfully confirmed") : BadRequest("operation failed");

        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("ChangeOrderStatus/{orderId:int}")]
        public async Task<IActionResult> ChangeOrderStatus(int orderId,OrderStatus status)
        {
            var command = new ChangeOrderStatusRequest(orderId,status);
            var result = await mediator.Send(command);

            return result ? Ok($"Order:{orderId} status changed  to `{status.ToString()}`") : BadRequest("An error occoured");
        }
        [HttpPost]
        [Route("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder([FromForm] OrderPostDTO dto_order)
        {
            var command = new PlaceOrderRequest(dto_order);
            var result = await mediator.Send(command);
            return result ? Ok($"Order created successfully") : BadRequest("An error occured");

        }
    }
}
