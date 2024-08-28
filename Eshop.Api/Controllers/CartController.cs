﻿using AutoMapper;
using eshop.DataAccess.Data;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.DataAccess.Services.Validators;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using Microsoft.AspNetCore.Authorization;
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
        [HttpPut]
        [Route("UpdateCartItem/{Id:int}")]
        public async Task<IActionResult> UpdateCartItem([FromRoute]CartPostDTO dto_cart)
        {
            var validate = new CartPostValidator();
            var result = validate.Validate(dto_cart);
            if (!result.IsValid)
                return BadRequest(result.Errors.ToString());

            if (!ModelState.IsValid)
                return BadRequest($"Invalid cart model");

            var cart = mapper.Map<Cart>(dto_cart);
            await uow.CartRepository.Update(cart);
            await uow.CommitAsync();

            return Ok($"Cart item {cart.Id} updated successfully");
        }
        [HttpPut]
        [Route("UpdateCartQuantity/{Id:int}/{ProductId:int}")]
        public async Task<IActionResult> UpdateCartQuantity([FromRoute]CartPostDTO dto_cart)
        {
            var validate = new CartPostValidator();
            var result = validate.Validate(dto_cart);
            if(!result.IsValid)
                return BadRequest(result.Errors.ToString());

            var cart = await uow.CartRepository.GetCartAsync(x => x.Id == dto_cart.Id && x.ProductId == dto_cart.ProductId);
            if (cart is not null)
            {
                await uow.CartRepository.IncreaseCount(cart, dto_cart.Count);
                await uow.CommitAsync();
                return Ok($"Cart item:{cart.Id} Product:{cart.ProductId} count updated successfully");
            }
            return BadRequest($"Cart item not found ");
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
                return BadRequest(new {Message = $"You Must logged in to access this feature"});
        }
        
    }
}
