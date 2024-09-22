using AutoMapper;
using eshop.DataAccess.Data;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands;
using Eshop.Api.Queries;
using Eshop.Api.Queries.Cart;
using Eshop.DataAccess.Services.Middleware;
using Eshop.DataAccess.Services.Validators;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Eshop.Api.Controllers
{
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Eshop-UI")]

    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("AddToCart")]
        public async Task<IActionResult> AddToCart(CartPostDTO dto_cart)
        {
            var command = new AddToCartRequest(dto_cart);
            var result = await _mediator.Send(command);
            return result ? Ok("Item added to cart successfully") : BadRequest();
        }
        [HttpDelete]
        [Route("DeleteCartItem/{Id:int}")]
        public async Task<IActionResult> DeleteCartItem([FromRoute] int Id)
        {
            var command = new DeleteCartItemRequest(Id);
            var result = await _mediator.Send(command);

            return result? Ok($"Cart item:{Id} deleted successfully"): BadRequest();
        }
        
        [HttpPut]
        [Route("IncreaseCartQuantity/{Id:int}/{ProductId:int}")]
        public async Task<IActionResult> IncreaseCartQuantity(int id, int productId,[FromBody] CartPostDTO dto_cart)
        {
            var command = new IncreaseCartQuantityRequest(id,productId,dto_cart);
            var result = await _mediator.Send(command);
            return result ? Ok($"Cart item:{id} Product:{productId} count updated successfully") : BadRequest();
        }
        [HttpPut]
        [Route("DecreaseCartQuantity/{Id:int}/{ProductId:int}")]
        public async Task<IActionResult> DecreaseCartQuantity(int id, int productId, [FromBody] CartPostDTO dto_cart)
        {
            var command = new DecreaseCartQuantityRequest(id,productId,dto_cart);
            var result = await _mediator.Send(command);
            return result ? Ok($"Cart item:{id} Product:{productId} count updated successfully") : BadRequest();

        }
        [HttpGet]
        [Route("Summery")]
        public async Task<IActionResult> Summery()
        {
            var query = new GetCartSummery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        
    }
}
