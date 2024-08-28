using AutoMapper;
using eshop.DataAccess.Data;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.DataAccess.Services.Validators;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eshop.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly AppDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public CartController(IUnitOfWork uow, IMapper mapper, AppDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        [Route("GetUserCart")]
        public async Task<IActionResult> GetUserCarts()
        {
            var userId = User.FindFirstValue("uid");
            if(userId is not null)
            {
                var carts = await uow.CartRepository.GetUserCart(userId,includes:"Product");
                if (carts is null)
                    return BadRequest($"User {userId} carts not found");

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
                                 && x.ProductId == dto_cart.ProductId,includes:"Product,ApplicationUser");

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

            uow.CartRepository.DeleteAsync(cart);
            await uow.CommitAsync();

            return Ok($"Cart item:{Id} deleted successfully");
        }
    }
}
