using AutoMapper;
using eshop.DataAccess.Data;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Eshop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        public HomeController(AppDbContext context, IUnitOfWork uow, IMapper mapper)
        {
            this.context = context;
            this.uow = uow;
            this.mapper = mapper;
        }
        [HttpGet]
        [Route("Index")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Index()
        {
            var products = await uow.ProductRepository.GetAllAsync(includes: "Category");
            if (products is null)
                return BadRequest($"Empty List no product found");

            var dto_products = mapper.Map<List<ProductDTO>>(products);
            return Ok(dto_products);
        }
        [HttpGet]
        [Route("ProductDetails/{pId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ProductDetails([FromRoute] int pId)
        {
            if (pId <= 0)
                return BadRequest($"Invalid id value:{pId}");
            
            var product = await uow.ProductRepository.GetAsync(pId,includes:"Category");
            if (product is null)
                return BadRequest($"Product with id:{pId} not found");

            var dto_product = mapper.Map<ProductDTO>(product);
            return Ok(dto_product);
        }
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        [Route("AddToCart")]
        public async Task<IActionResult> AddToCart(ShoppingCart cart)
        {
            var existingUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
        }*/
    }
}
