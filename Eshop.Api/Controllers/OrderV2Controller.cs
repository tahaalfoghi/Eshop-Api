using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.Api.Queries.Order;
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
    [ApiExplorerSettings(GroupName = "v2")]
    public class OrderV2Controller:ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderV2Controller(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [Route("UserOrders")]
        public async Task<IActionResult> GetUserOrders([FromQuery]RequestParameter requestParameter)
        {
            var query = new GetOrdersQuery(requestParameter);
            var result = _mediator.Send(query);
            return Ok(result);
        }
    }
}
