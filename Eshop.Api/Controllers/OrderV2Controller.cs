using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.DataAccess.Services.Middleware;
using Eshop.DataAccess.Services.Requests;
using Eshop.Models.DTOModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eshop.Api.Controllers
{
    
    [Authorize]
    [Route("api/{v:apiversion}/orders")]
    [ApiController]
    public class OrderV2Controller:ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public OrderV2Controller(IMediator mediator, IUnitOfWork uow, IMapper mapper)
        {
            _mediator = mediator;
            _uow = uow;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("UserOrders")]
        public async Task<IActionResult> GetUserOrders([FromQuery]RequestParameter requestParameter)
        {
            var userId = User.FindFirstValue("uid");
            if (userId is null)
                throw new BadRequestException("Invalid user id");

            var user = await _uow.UsersRepository.GetUser(userId);
            if (user is null)
                throw new NotFoundException("No user signed in");

            var orders = await _uow.OrderRepository.GetAllAsync(requestParameter, includes:"ApplicationUser");
            if (orders is null)
                throw new NotFoundException("No orders found");

            var ordersDto = _mapper.Map<IEnumerable<OrderDTO>>(orders);

            return Ok(ordersDto);
        }
    }
}
