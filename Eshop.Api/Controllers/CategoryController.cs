﻿using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Models.Models;
using Eshop.Models.DTOModels;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Eshop.Models;
using Eshop.DataAccess.Services.Validators;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Eshop.Api.Queries;
using Eshop.Api.Commands;
using Eshop.Api.Handlers;

namespace Eshop.Api.Controllers
{
    [Authorize(Roles ="Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> logger;
        private readonly IMediator mediator;

        public CategoryController(ILogger<CategoryController> logger, IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }
        [HttpGet]
        [Route("Categories")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCategories()
        {
            var query = new GetCategoriesQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpGet]
        [Route("Categories/{categoryId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCategory(int categoryId)
        {

            var query = new GetCategoryQuery(categoryId);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpGet]
        [Route("CategoriesByFilter")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllByFilter([FromQuery] TableSearch search)
        {
            var query = new GetCategoriesByFilterQuery(search);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpGet]
        [Route("CategoryByFilter/{search}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSingleByFilter(TableSearch search)
        {
            var query = new GetCategoryByFilterQuery(search);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpPost]
        [Route("CreateCatgory")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateCategory(CategoryPostDTO dto_category)
        {
            var command = new CreateCategoryRequest(dto_category);
            var result = await mediator.Send(command);

            return CreatedAtAction(nameof(GetCategory), new { categoryId = result.Id }, result);
        }
        [HttpDelete]
        [Route("DeleteCategory/{categoryId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var command = new DeleteCategoryRequest(categoryId);
            var result = await mediator.Send(command);
            return result ? Ok($"Category [{categoryId}] deleted successfully") : BadRequest();
        }
        [HttpPut]
        [Route("UpdateCategory/{categoryId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateCategory(CategoryPostDTO dto_category)
        {
            var command = new UpdateCategoryRequest(dto_category);
            var result = await mediator.Send(command);

            return Ok($"Category updated successfully");
        }
        [HttpPatch]
        [Route("UpdatePatchCategory/{categoryId:int}")]
        public async Task<IActionResult> UpdatePath(int categoryId, [FromBody]JsonPatchDocument<CategoryPostDTO> patch)
        {
            
            var command = new UpdatePatchCategoryRequest(categoryId, patch);
            var result = await mediator.Send(command);
            return result ? Ok("Product updated successfully"):BadRequest();
        }
    }
}
