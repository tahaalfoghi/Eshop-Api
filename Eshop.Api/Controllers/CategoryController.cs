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
        
        public CategoryController(IUnitOfWork uow, ILogger<CategoryController> logger, IMapper mapper)
        {
            this.uow = uow;
            this.logger = logger;
            this.mapper = mapper;
        }
        [HttpGet]
        [Route("GetAllCategories")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllCategories()
        {
            logger.LogInformation("GetAllCategories endpoint");
            var categories = await uow.CategoryRepository.GetAllAsync(includes:"Supplier");

            if(categories is null)
            {
                throw new Exception("**ERROR IN CategoryController GetAllCategories endpoint\r\n records not found");
            }

            var dto_categories = mapper.Map<List<CategoryDTO>>(categories);
            return Ok(dto_categories);
        }
        [HttpGet]
        [Route("GetById/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetById(int id)
        {
            logger.LogInformation($"Get category by id {id}");
            var category = await uow.CategoryRepository.GetByIdAsync(id, includes: "Supplier");
            if(category is null)
            {
                throw new Exception($"**ERROR IN CategoryController GetById endpoint**\r\n");
            }
            var dto_category = mapper.Map<CategoryDTO>(category);
            return Ok(dto_category);
        }
        [HttpGet]
        [Route("GetAllByFilter")]
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
        [Route("GetSingleByFilter/{search}")]
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
            if (!ModelState.IsValid)
            {
                var error_message = $"**ERROR IN CategoryController at CreateCategory endpoint\r\n" +
                                    $"Invalid input for:{{ Name:{dto_category.Name} Supplier:{dto_category.SupplierId} }}";
                throw new Exception(error_message);
            }

            var validate = new CategoryValidator();
            var result = validate.Validate(dto_category);
            if (!result.IsValid)
            {
                return BadRequest($"Invalid input for category: {result.ToString()}");
            }
            var category = mapper.Map<Category>(dto_category);  
            await uow.CategoryRepository.CreateAsync(category);
            await uow.CommitAsync();
            
            return Ok(category.Id);
        }
        [HttpDelete]
        [Route("DeleteCategory/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (id <= 0)
            {
                var error_message = $"ERROR IN ERROR IN CategoryController at CreateCategory endpoint\r\n" +
                                    $"Invalid id:{id} ";
                throw new Exception(error_message);
            }
            var category = await uow.CategoryRepository.GetByIdAsync(id);
            if(category is null)
            {
                var error_message = $"ERROR IN ERROR IN CategoryController at CreateCategory endpoint\r\n" +
                                    $"Category with id:{id} not found ";
                throw new Exception(error_message);
            }
            uow.CategoryRepository.DeleteAsync(category);
            await uow.CommitAsync();
            return Ok(category.Id);
        }
        [HttpPut]
        [Route("UpdateCategory/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateCategory(int id,CategoryPostDTO dto_category)
        {
            var category = await uow.CategoryRepository.GetByIdAsync(id);
            if(category is null)
            {
                var error_message = $"ERROR IN  CategoryController at UpdateCategory endpoint\r\n" +
                                   $"Invalid id:{id} ";
                throw new Exception(error_message);
            }
            if (!ModelState.IsValid)
            {
                var error_message = $"ERROR IN CategoryController at UpdateCategory endpoint\r\n" +
                                    $"Invalid input for the category: Name:{dto_category.Name} Supplier:{dto_category.SupplierId} ";
                throw new Exception(error_message);
            }
            var validate = new CategoryValidator();
            var result = validate.Validate(dto_category);
            if (!result.IsValid)
            {
                return BadRequest($"Invalid input for category: {result.ToString()}");
            }
            await uow.CategoryRepository.UpdateAsync(id, dto_category);
            await uow.CommitAsync();
            
            return Ok(category.Id);
        }
        [HttpPatch]
        [Route("UpdatePatchCategory/{id:int}")]
        public async Task<IActionResult> UpdatePath(int id,[FromBody]JsonPatchDocument<CategoryPostDTO> patch)
        {
            var existingcategory = await uow.CategoryRepository.GetByIdAsync(id);
            if(existingcategory is null)
            {
                return NotFound($"Category with id:{id} is not found");
            }
            if(id<=0)
            {
                throw new Exception($"Invalid id:{id}");
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
            await uow.CategoryRepository.UpdatePatchAsync(id,category);
            await uow.CommitAsync();

            return Ok(id);
        }
    }
}
