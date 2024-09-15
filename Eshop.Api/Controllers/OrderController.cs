using AutoMapper;
using eshop.DataAccess.Data;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Requests;
using Eshop.DataAccess.Services.Validators;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eshop.Api.Controllers
{
    
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Eshop-UI")]

    public class OrderController:ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IMediator mediator;
        private AppDbContext context;
        public OrderController(IUnitOfWork uow, IMapper mapper, IMediator mediator, AppDbContext context)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.mediator = mediator;
            this.context = context;
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
        [Authorize(Roles = "Admin")]
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
        [HttpPost]
        [Route("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder([FromForm] OrderPostDTO dto_order)
        {

            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    var userId = User.FindFirstValue("uid");
                    if (string.IsNullOrEmpty(userId))
                        return BadRequest(new { Message = "No user has Signed In" });

                    var user = await uow.UsersRepository.GetUser(userId);
                    if (user is null)
                        return BadRequest(new { Message = $"User not found" });

                    var validate = new OrderValidator();
                    var result = validate.Validate(dto_order);
                    if (!result.IsValid)
                        return BadRequest($"Invalid order model");

                    var order = mapper.Map<Order>(dto_order);
                    order.ApplicationUser = user;
                    if (order.ApplicationUser is null)
                        return BadRequest("Invalid User");

                    order.OrderDate = DateTime.Now;
                    order.Status = OrderStatus.Approved.ToString();
                    var currentCart = await uow.CartRepository.GetUserCart(userId, "Product,ApplicationUser");
                    if (currentCart is null)
                        return BadRequest($"Cart is empty");

                    order.TotalPrice = currentCart.Sum(x => x.Count * x.Product.Price);

                    await uow.OrderRepository.CreateAsync(order);
                    await uow.CommitAsync();

                    foreach (var item in currentCart)
                    {
                        var orderDetail = new OrderDetail()
                        {
                            OrderId = order.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Count,
                            UnitPrice = item.Product.Price
                        };
                        var validte = new OrderDetailValidator();
                        var ordDetailResult = validte.Validate(orderDetail);
                        if (!ordDetailResult.IsValid)
                            return BadRequest($"Opreration failed for orderDetail model:{result.Errors.ToString()}");
                        await uow.OrderDetailRepository.CreateAsync(orderDetail);
                        await uow.CommitAsync();

                    }
                    trans.Commit();
                    return Ok($"Order: {order.Id} created successfully");

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return BadRequest($"Exception: {ex.Message}, StackTrace: {ex.StackTrace}, InnerException: {ex.InnerException?.Message}");
                }
            }
        }
    }
}
