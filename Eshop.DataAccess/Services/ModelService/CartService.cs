using AutoMapper;
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
    public sealed class CartService : ICartService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task AddToCart(CartPostDTO cartPostDTO)
        {
            try
            {
                var validate = new CartPostValidator();
                var result = validate.Validate(cartPostDTO);
                if (result is null)
                    throw new InvalidModelException(string.Join(",",result));

                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue("uid");
                if (userId is not null)
                {
                    var existsCart = await _uow.CartRepository
                                 .GetCartAsync(x => x.Id == cartPostDTO.Id && x.UserId == userId
                                 && x.ProductId == cartPostDTO.ProductId, includes: "Product,ApplicationUser");

                    if (existsCart is null)
                    {
                        var cart = _mapper.Map<Cart>(cartPostDTO);
                        cart.UserId = userId;
                        await _uow.CartRepository.CreateAsync(cart);
                    }
                    else
                    {
                        await _uow.CartRepository.IncreaseCount(existsCart, cartPostDTO.Count);
                    }
                    await _uow.CommitAsync();

                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Invalid operation, {ex.Message}");
            }
        }

        public Task DecreaseCartQuantity(int id, int productId, CartPostDTO cartPostDTO)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCartItem(int cartId)
        {
            throw new NotImplementedException();
        }

        public Task IncreaseCartQuantity(int id, int productId, CartPostDTO cartPostDTO)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Cart>> Summery()
        {
            throw new NotImplementedException();
        }
    }
}
