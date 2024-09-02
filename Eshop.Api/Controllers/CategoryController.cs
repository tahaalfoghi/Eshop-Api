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
            var categories = await uow.CategoryRepository.GetAllByFilterAsync(search, includes: "Supplier");

            if (categories is null)
            {
                throw new Exception($"**ERROR IN CategoryController at GetAllByFilter endpoint\r\n Filter:{{{search.Id} {search.Name}");
            }

            var dto_categories = mapper.Map<List<Models.DTOModels.CategoryDTO>>(categories);
            return Ok(dto_categories);
        }
        [HttpGet]
        [Route("CategoryByFilter/{search}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSingleByFilter(TableSearch search)
        {
            var category = await uow.CategoryRepository.GetFirstOrDefaultAsync(search);
            if (category is null)
            {
                var errorMessage = $"**ERROR IN CategoryController at GetSingleByFilter endpoint**\r\n" +
                                   $"filter:{search}";
                throw new Exception(errorMessage);
            }
            var dto_category = mapper.Map<CategoryDTO>(category);

            return Ok(dto_category);
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
