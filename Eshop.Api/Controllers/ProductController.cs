using AutoMapper;
using Eshop.Api.Commands;
using Eshop.Api.Commands.Create;
using Eshop.Api.Commands.Delete;
using Eshop.Api.Commands.Update;
using Eshop.Api.Queries;
using Eshop.Api.Queries.Product;
using Eshop.DataAccess.Services.Requests;
using Eshop.Models;
using Eshop.Models.DTOModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Eshop-UI")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProductController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        [Route("Products", Name = "GetProducts")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllProducts([FromQuery] RequestParameter requestParameter)
        {
            var query = new GetProductsQuery(requestParameter);
            var result = await mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("Products/{productId:int}", Name = "GetProduct")]
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
        public async Task<IActionResult> GetAllProductsByFilter([FromQuery] ProductRequestParamater param)
        {

            var query = new GetProductsByFilterQuery(param);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        
        [HttpPost]
        [Route("CreateProduct", Name = "CreateProduct")]
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
        [Route("DeleteProduct/{productId:int}", Name = "DeleteProduct")]
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
        [Route("UpdateProduct/{productId:int}", Name = "UpdateProduct")]
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
        [Route("UpdatePatch/{productId:int}", Name = "UpdatePatchProduct")]
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
        [HttpOptions]
        public async Task<IActionResult> GetProductsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST,DELETE,UPDATE,PATCH");
            return Ok();
        }
    }
}
