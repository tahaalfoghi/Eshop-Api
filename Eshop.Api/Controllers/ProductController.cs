using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Validators;
using Eshop.Models;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.Sockets;

namespace Eshop.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public ProductController(IUnitOfWork uow, IMapper mapper, IMediator mediator)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.mediator = mediator;
        }
        [HttpGet]
        [Route("Products")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllProducts()
        {
            var query = new GetProductsQuery();
            var result = await mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("Products/{productId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetProduct([FromRoute]int productId)
        {
            var query = new GetProductQuery(productId);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpGet]
        [Route("ProductsByFilter")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllProductsByFilter([FromQuery] TableSearch search)
        {
            var products = await uow.ProductRepository.GetAllByFilterAsync(search, includes: "Category");
            if (products is null)
            {
                return BadRequest($"Products not found with filter:{search.ToString()}");
            }
            var dto_products = mapper.Map<List<ProductDTO>>(products);

            return Ok(dto_products);
        }
        [HttpGet]
        [Route("ProductByFilter")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSingleProductByFilter([FromForm] TableSearch search)
        {
            var product = await uow.ProductRepository.GetFirstOrDefaultAsync(search, includes: "Category");
            if (product is null)
                return BadRequest($"Produc with search:{search.ToString()} not found");

            var dto_product = mapper.Map<ProductDTO>(product);
            return Ok(dto_product);
        }
        [HttpPost]
        [Route("CreateProduct")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateProduct([FromForm] ProductPostDTO dto_product)
        {
            
            var command = new CreateProductRequest(dto_product);
            var result = await mediator.Send(command);

            return CreatedAtAction(nameof(GetProduct), new {ProductId = result.Id}, result);
        }
        [HttpDelete]
        [Route("DeleteProduct/{productId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteProduct([FromRoute]int productId)
        {
            
            var command = new DeleteProductRequest(productId);
            var result = await mediator.Send(command);
            return result ? Ok($"Product deleted successfully") : BadRequest();
        }
        [HttpPut]
        [Route("UpdateProduct/{productId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateProduct(int productId, [FromForm] ProductPostDTO dto_product)
        {
            /*if (productId <= 0)
                return BadRequest($"Invalid id:{productId} value");

            var validate = new ProductValidator();
            var result = validate.Validate(dto_product);
            if (!result.IsValid)
                return BadRequest($"Invalid input: {result.ToString()}");

            var existingProduct = await uow.ProductRepository.GetByIdAsync(productId, includes:"Category");
            if (existingProduct is null)
                return BadRequest($"Product with Id:{productId} not found");

            string? ImgUrl = string.Empty;
            if (dto_product.ImageUrl is not null)
            {
                var path = Path.Combine("wwwroot", "images", dto_product.ImageUrl.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await dto_product.ImageUrl.CopyToAsync(stream);
                }
                ImgUrl = $"/images/{dto_product.ImageUrl.FileName}";
            }
            existingProduct = new Product
            {
                Name = dto_product.Name,
                Description = dto_product.Description,
                Price = dto_product.Price,
                ImageUrl = ImgUrl,
                CategoryId = dto_product.CategoryId,
            };

            await uow.ProductRepository.UpdateAsync(productId, existingProduct);
            await uow.CommitAsync();

            return Ok($"Product {productId} updated successfully");*/
            var command = new UpdateProductRequest(productId, dto_product);
            var result = await mediator.Send(command);
            return result ? Ok("Product Updated successfully") : BadRequest();    
        }
        [HttpPatch]
        [Route("UpdatePatch/{productId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePatch(int productId, [FromBody] JsonPatchDocument<ProductPostDTO> patch)
        {
            if (productId <= 0)
                return BadRequest($"Invalid Id:{productId}");

            

            var existingProduct = await uow.ProductRepository.GetByIdAsync(productId, includes:"Category");
            if (existingProduct is null)
                return BadRequest($"Product with id:{productId} not found");

            var dto_product = mapper.Map<ProductPostDTO>(existingProduct);
            patch.ApplyTo(dto_product, ModelState);

            string? ImgUrl = string.Empty;
            if (dto_product.ImageUrl is not null)
            {
                var path = Path.Combine("wwwroot", "images", dto_product.ImageUrl.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await dto_product.ImageUrl.CopyToAsync(stream);
                }
                ImgUrl = $"/images/{dto_product.ImageUrl.FileName}";
            }
            var validate = new ProductValidator();
            var result = validate.Validate(dto_product);
            if (!result.IsValid)
                return BadRequest($"Invalid input: {result.ToString()}");

            var product = new Product
            {
                Name = dto_product.Name,
                Description = dto_product.Description,
                Price = dto_product.Price,
                ImageUrl = ImgUrl,
                CategoryId = dto_product.CategoryId,
            };


            await uow.ProductRepository.UpdatePatch(productId, product);
            await uow.CommitAsync();

            return Ok($"Product {productId} patch update success");
        }
    }
}
