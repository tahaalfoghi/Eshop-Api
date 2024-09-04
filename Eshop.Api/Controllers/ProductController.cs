using AutoMapper;
using Eshop.Api.Commands;
using Eshop.Api.Queries;
using Eshop.Models;
using Eshop.Models.DTOModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProductController(IMediator mediator)
        {
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

            var query = new GetProductsByFilterQuery(search);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpGet]
        [Route("ProductByFilter")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSingleProductByFilter([FromForm] TableSearch search)
        {
            var query = new GetProductByFilterQuery(search);
            var result = await mediator.Send(query);
            return Ok(result);
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
           
            var command = new UpdatePatchProductRequest(productId, patch);
            var result = await mediator.Send(command);
            return result ? Ok($"Product {productId} patch update success") : BadRequest();
        }
    }
}
