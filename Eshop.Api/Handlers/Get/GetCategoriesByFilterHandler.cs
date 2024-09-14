using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Links;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Handlers.Get
{
    public class GetCategoriesByFilterHandler : IRequestHandler<GetCategoriesByFilterQuery, IEnumerable<CategoryDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly ILinksService linksService;
        private readonly IHttpContextAccessor httpContextAccessor;
        public GetCategoriesByFilterHandler(IUnitOfWork uow, IMapper mapper, ILinksService linksService, IHttpContextAccessor httpContextAccessor)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.linksService = linksService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<CategoryDTO>> Handle(GetCategoriesByFilterQuery request, CancellationToken cancellationToken)
        {
            var categories = await uow.CategoryRepository.GetAllByFilterAsync(request.Params, includes: "Supplier");
            if (categories is null || categories.Count() == 0)
                throw new NotFoundException($"no category exists with this filter:{request.Params.ToString()}");

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
