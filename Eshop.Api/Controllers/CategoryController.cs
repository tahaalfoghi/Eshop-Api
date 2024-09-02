using AutoMapper;
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
        private readonly IUnitOfWork uow;
        private readonly ILogger<CategoryController> logger;
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public CategoryController(IUnitOfWork uow, ILogger<CategoryController> logger, IMapper mapper, IMediator mediator)
        {
            this.uow = uow;
            this.logger = logger;
            this.mapper = mapper;
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
        public async Task<IActionResult> GetAllByFilter([FromQuery]TableSearch search)
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
            var existingcategory = await uow.CategoryRepository.GetByIdAsync(categoryId);
            if(existingcategory is null)
            {
                return NotFound($"Category with id:{categoryId} is not found");
            }
            if(categoryId <= 0)
            {
                throw new Exception($"Invalid id:{categoryId}");
            }

            var dto_category = mapper.Map<CategoryPostDTO>(existingcategory);
            patch.ApplyTo(dto_category,ModelState);

            var validate = new CategoryValidator();
            var result = validate.Validate(dto_category);
            if (!result.IsValid)
            {
                return BadRequest($"Invalid input for category: {result.ToString()}");
            }
            var category = mapper.Map<Category>(dto_category);
            await uow.CategoryRepository.UpdatePatchAsync(category);
            await uow.CommitAsync();

            return Ok(categoryId);
        }
    }
}
