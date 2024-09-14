using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Links;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Handlers.GetById
{
    public class GetCategoryHandler : IRequestHandler<GetCategoryQuery, CategoryDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly ILinksService linksService;
        private readonly IHttpContextAccessor httpContextAccessor;
        public GetCategoryHandler(IUnitOfWork uow, IMapper mapper, ILinksService linksService, IHttpContextAccessor httpContextAccessor)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.linksService = linksService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<CategoryDTO> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await uow.CategoryRepository.GetByIdAsync(request.CategoryId, includes: "Supplier");
            if (category is null)
                throw new NotFoundException($"Category [{request.CategoryId}] not exist");

            var dtoCategory = mapper.Map<CategoryDTO>(category);
            AddLinks(dtoCategory);
            return dtoCategory;
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
