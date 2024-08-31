﻿using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.DataAccess.Services.Validators;
using Eshop.Models;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.Sockets;

namespace Eshop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public ProductController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }
        [HttpGet]
        [Route("Products")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await uow.ProductRepository.GetAllAsync(includes:"Category");
            if(products is null)
            {
                return NotFound("Products not found");
            }
            var dto_products = mapper.Map<List<ProductDTO>>(products);
            return Ok(dto_products);
        }
        [HttpGet]
        [Route("Product/{Id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetProduct([FromRoute]int Id)
        {
            if (Id <= 0)
                return BadRequest($"Invalid Id:{Id}");

            var product = await uow.ProductRepository.GetByIdAsync(Id,includes:"Category");
            if(product is null)
            {
                return BadRequest($"Product with id:{Id} not found");
            }
            var dto_product = mapper.Map<ProductDTO>(product);
            return Ok(dto_product);
        }
        [HttpGet]
        [Route("ProductsByFilter")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllProductsByFilter([FromQuery]TableSearch search)
        {
            var products = await uow.ProductRepository.GetAllByFilterAsync(search, includes: "Category");
            if( products is null)
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
            var product =  await uow.ProductRepository.GetFirstOrDefaultAsync(search, includes: "Category");
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
            var validate = new ProductValidator();
            var result = validate.Validate(dto_product);
            if (!result.IsValid)
                return BadRequest($"Invalid input: {result.ToString()}");

            if (!ModelState.IsValid)
            {
                return BadRequest($"Invalid data for product: {dto_product.ToString()} {ModelState}");
            }
            string? ImgUrl =string.Empty;
            if(dto_product.ImageUrl is not null)
            {
                var path = Path.Combine("wwwroot", "images", dto_product.ImageUrl.FileName);
                using(var stream = new FileStream(path, FileMode.Create))
                {
                    await dto_product.ImageUrl.CopyToAsync(stream);
                }
                ImgUrl = $"/images/{dto_product.ImageUrl.FileName}";
            }
            var product = new Product
            {
                Name = dto_product.Name,
                Description = dto_product.Description,
                Price = dto_product.Price,
                ImageUrl = ImgUrl,
                CategoryId = dto_product.CategoryId,
            };

            await uow.ProductRepository.CreateAsync(product);
            await uow.CommitAsync();

            return Ok($"Product {product.Id} created successfully");
        }
        [HttpDelete]
        [Route("DeleteProduct/{Id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteProduct([FromRoute]int Id)
        {
            if (Id <= 0)
                return BadRequest($"Invalid:{Id}");

            var product =  await uow.ProductRepository.GetByIdAsync(Id,includes:"Category");
            if (product is null)
                return BadRequest($"Product with id:{Id} is not found");

            uow.ProductRepository.DeleteAsync(product);
            await uow.CommitAsync();

            return Ok($"Product with id:{Id} deleted successfully");
        }
        [HttpPut]
        [Route("UpdateProduct/{Id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateProduct(int Id, [FromForm] ProductPostDTO dto_product)
        {
            if (Id <= 0)
                return BadRequest($"Invalid id:{Id} value");

            var validate = new ProductValidator();
            var result = validate.Validate(dto_product);
            if (!result.IsValid)
                return BadRequest($"Invalid input: {result.ToString()}");

            var existingProduct = await uow.ProductRepository.GetByIdAsync(Id,includes:"Category");
            if (existingProduct is null)
                return BadRequest($"Product with Id:{Id} not found");

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

            await uow.ProductRepository.UpdateAsync(Id,existingProduct);
            await uow.CommitAsync();

            return Ok($"Product {Id} updated successfully");
        }
        [HttpPatch]
        [Route("UpdatePatch/{Id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePatch(int Id, [FromBody] JsonPatchDocument<ProductPostDTO> patch)
        {
            if (Id <= 0)
                return BadRequest($"Invalid Id:{Id}");

            

            var existingProduct = await uow.ProductRepository.GetByIdAsync(Id,includes:"Category");
            if (existingProduct is null)
                return BadRequest($"Product with id:{Id} not found");

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


            await uow.ProductRepository.UpdatePatch(Id,product);
            await uow.CommitAsync();

            return Ok($"Product {Id} patch update success");
        }
    }
}
