using AutoMapper;
using eshop.DataAccess.Data;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.DataAccess.Services.Middleware;
using Eshop.DataAccess.Services.Validators;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Services.ModelService
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(IUnitOfWork uow, IMapper mapper, AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task ChangeOrderStatus(int orderId, OrderStatus status)
        {
            if (orderId <= 0)
                throw new BadRequestException($"Invalid Id value:{orderId}");

            if (string.IsNullOrEmpty(status.ToString()))
                throw new BadRequestException($"Order status value is null");

            var order = await _uow.OrderRepository.GetByIdAsync(orderId);
            if (order is null)
                throw new BadRequestException($"Order with id:{orderId} is not found");

            var oldStatus = order.Status;
            _uow.OrderRepository.ChangeStatus(order, status);
            await _uow.CommitAsync();

            if (order.Status.Equals("Canceled"))
            {
                var trans = await _uow.TransactionRepository.GetAsync(x => x.OrderId == order.Id);
                _uow.TransactionRepository.Delete(trans);
            }
        }

        public async Task OrderConfirmation(int orderId)
        {
            if (orderId <= 0)
                throw new BadRequestException($"Invalid orderId [{orderId}]");

            var order = await _uow.OrderRepository.GetByIdAsync(orderId);
            if (order is null)
                throw new NotFoundException($"order [{orderId}] is not found");

            order.Status = OrderStatus.Pending.ToString();
            var transaction = new Transaction()
            {
                OrderId = order.Id,
                UserId = order.ApplicationUser.Id,
                Amount = order.TotalPrice,
                CreatedAt = DateTime.UtcNow,
            };
            
            await _uow.TransactionRepository.CreateAsync(transaction);
            var carts = await _uow.CartRepository.GetCartsAsync(x=>x.UserId == order.ApplicationUser.Id);
            _uow.CartRepository.DeleteRangeAsync(carts);
            await _uow.CommitAsync();
        }

        public async Task PlaceOrder(OrderPostDTO orderPostDTO)
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var userId = _httpContextAccessor.HttpContext.User.FindFirstValue("uid");
                    if (string.IsNullOrEmpty(userId))
                        throw new BadRequestException("No user has Signed In" );

                    var user = await _uow.UsersRepository.GetUser(userId);
                    if (user is null)
                        throw new BadRequestException($"User not found");

                    var validate = new OrderValidator();
                    var result = validate.Validate(orderPostDTO);
                    if (!result.IsValid)
                        throw new BadRequestException($"Invalid order model");

                    var order = _mapper.Map<Order>(orderPostDTO);
                    order.ApplicationUser = user;
                    if (order.ApplicationUser is null)
                        throw new BadRequestException("Invalid User");

                    order.OrderDate = DateTime.Now;
                    order.Status = OrderStatus.Approved.ToString();
                    var currentCart = await _uow.CartRepository.GetUserCart(userId, "Product,ApplicationUser");
                    if (currentCart is null)
                        throw new BadRequestException($"Cart is empty");

                    order.TotalPrice = currentCart.Sum(x => x.Count * x.Product.Price);

                    await _uow.OrderRepository.CreateAsync(order);
                    await _uow.CommitAsync();

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
                            throw new BadRequestException($"Opreration failed for orderDetail model:{result.Errors.ToString()}");
                        await _uow.OrderDetailRepository.CreateAsync(orderDetail);
                        await _uow.CommitAsync();

                    }
                    trans.Commit();

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new BadRequestException($"Exception: {ex.Message}, StackTrace: {ex.StackTrace}, InnerException: {ex.InnerException?.Message}");
                }
            }
        }
    }
}
