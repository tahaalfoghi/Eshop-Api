using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries.Category;
using Eshop.DataAccess.Services.Links;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;
using Newtonsoft.Json;

namespace Eshop.Api.Handlers.Get
{
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly ILinksService linksService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetCategoriesHandler(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILinksService linksService)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.linksService = linksService;
        }

        public async Task<IEnumerable<CategoryDTO>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await uow.CategoryRepository.GetAllAsync(request.requestParameter, includes: "Supplier");
            if (categories is null || categories.Count() == 0)
                throw new NotFoundException("No category exists in database");

            //httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(categroies.MetaData));
            var categoriesDto = mapper.Map<IEnumerable<CategoryDTO>>(categories);
            foreach (var category in categoriesDto)
            {
                AddLinks(category);
            }
            return categoriesDto;

        }
        private void AddLinks(CategoryDTO category)
        {
            category.Links.Add(
                linksService.Generate("GetCategory", new { categoryId = category.Id }, "self", "GET"));

            category.Links.Add(
               linksService.Generate("UpdateCategory", new { categoryId = category.Id }, "update-category", "PUT"));

            category.Links.Add(
               linksService.Generate("DeleteCategory", new { categoryId = category.Id }, "delete-category", "DELETE"));

            category.Links.Add(
               linksService.Generate("UpdatePatchCategory", new { categoryId = category.Id }, "updatePatch-category", "Patch"));
        }
    }
}
