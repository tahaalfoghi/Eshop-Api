using AutoMapper;
using eshop.DataAccess.Data;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.DataAccess.Services.Middleware;
using Eshop.DataAccess.Services.Validators;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Eshop.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Eshop-UI")]

    public class CartController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly AppDbContext context;
        public CartController(IUnitOfWork uow, IMapper mapper, AppDbContext context)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.context = context;
        }
        [HttpGet]
        [Route("Carts")]
        public async Task<IActionResult> GetUserCarts()
        {
            var userId = HttpContext.User.FindFirstValue("uid");
            if (userId is not null)
            {
                var carts = await uow.CartRepository.GetUserCart(userId, includes: "Product");
                if (carts is null || carts.Count() <=0)
                    return NotFound($"Your cart is empty");

                var dto_carts = mapper.Map<List<CartDTO>>(carts);
                return Ok(dto_carts);
            }
            return NotFound($"User not found");


        }
        [HttpPost]
        [Route("AddToCart")]
        public async Task<IActionResult> AddToCart(CartPostDTO dto_cart)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest($"Invalid cart model");

                var userId = User.FindFirstValue("uid");
                if (userId is not null)
                {
                    var existsCart = await uow.CartRepository
                                 .GetCartAsync(x => x.Id == dto_cart.Id && x.UserId == userId
                                 && x.ProductId == dto_cart.ProductId, includes: "Product,ApplicationUser");

                    if (existsCart is null)
                    {
                        var cart = mapper.Map<Cart>(dto_cart);
                        cart.UserId = userId;
                        await uow.CartRepository.CreateAsync(cart);
                    }
                    else
                    {
                        await uow.CartRepository.IncreaseCount(existsCart, dto_cart.Count);
                    }
                    await uow.CommitAsync();

                    return Ok($"Item added to cart successfully: {{ ProductId:{dto_cart.ProductId} Count:{dto_cart.Count}");
                }
                return BadRequest($"Error happend while getting the userId");

            }
            catch (Exception ex)
            {
                throw new Exception($"Invalid operation, {ex.Message}");
            }
        }
        [HttpDelete]
        [Route("DeleteCartItem/{Id:int}")]
        public async Task<IActionResult> DeleteCartItem([FromRoute] int Id)
        {
            if (Id <= 0)
                return BadRequest($"Invalid id:{Id} value");

            var cart = await uow.CartRepository.GetByIdAsync(Id);
            if (cart is null)
                return BadRequest($"Cart with id:{Id} not found");

            uow.CartRepository.Delete(cart);
            await uow.CommitAsync();

            return Ok($"Cart item:{Id} deleted successfully");
        }
        
        [HttpPut]
        [Route("IncreaseCartQuantity/{Id:int}/{ProductId:int}")]
        public async Task<IActionResult> IncreaseCartQuantity(int Id, int ProductId,[FromBody] CartPostDTO dto_cart)
        {
            var validate = new CartPostValidator();
            var result = validate.Validate(dto_cart);
            if (!result.IsValid)
                return BadRequest(result.Errors.ToString());

            var cart = await uow.CartRepository.GetCartAsync(x => x.Id == Id && x.ProductId == ProductId);
            if (cart is null)
                throw new NotFoundException();

            await uow.CartRepository.IncreaseCount(cart, dto_cart.Count);
            await uow.CommitAsync();
            return Ok($"Cart item:{cart.Id} Product:{cart.ProductId} count updated successfully");

        }
        [HttpPut]
        [Route("DecreaseCartQuantity/{Id:int}/{ProductId:int}")]
        public async Task<IActionResult> DecreaseCartQuantity(int Id, int ProductId, [FromBody] CartPostDTO dto_cart)
        {
            var validate = new CartPostValidator();
            var result = validate.Validate(dto_cart);
            if (!result.IsValid)
                return BadRequest(result.Errors.ToString());

            var cart = await uow.CartRepository.GetCartAsync(x => x.Id == Id && x.ProductId == ProductId);
            if (cart is null)
                throw new NotFoundException();

            await uow.CartRepository.DecreaseCount(cart, dto_cart.Count);
            await uow.CommitAsync();
            return Ok($"Cart item:{cart.Id} Product:{cart.ProductId} count updated successfully");

        }
        [HttpGet]
        [Route("Summery")]
        public async Task<IActionResult> Summery()
        {
            var userId = User.FindFirstValue("uid");
            if (userId is not null)
            {
                var carts = mapper.Map<List<CartDTO>>(await uow.CartRepository.GetUserCart(userId, includes: "Product"));
                if (carts is not null)
                {
                    return Ok(carts);
                }
                else
                    return BadRequest(new { Message = $"Cart is empty" });
            }
            else
                return BadRequest(new { Message = $"You Must logged in to access this feature" });
        }
        
    }
}
