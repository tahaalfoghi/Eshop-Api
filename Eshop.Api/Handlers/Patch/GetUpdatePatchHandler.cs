using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands.Update;
using Eshop.DataAccess.Services.Middleware;
using Eshop.DataAccess.Services.Validators;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using MediatR;

namespace Eshop.Api.Handlers.Patch
{
    public class GetUpdatePatchHandler : IRequestHandler<UpdatePatchCategoryRequest, bool>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetUpdatePatchHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(UpdatePatchCategoryRequest request, CancellationToken cancellationToken)
        {
            if (request.categoryId <= 0)
                throw new BadRequestException($"Invalid id:[{request.categoryId}]");

            var existingcategory = await uow.CategoryRepository.GetByIdAsync(request.categoryId);
            if (existingcategory is null)
                throw new NotFoundException($"category:[{request.categoryId}] doesn't exists");


            var dto_category = mapper.Map<CategoryPostDTO>(existingcategory);
            request.patch.ApplyTo(dto_category);

            var validate = new CategoryValidator();
            var result = validate.Validate(dto_category);
            if (!result.IsValid)
                throw new InvalidModelException($"Invalid model {string.Join(",", result.Errors)}");
            var category = mapper.Map<Category>(dto_category);
            await uow.CategoryRepository.UpdatePatchAsync(category);
            await uow.CommitAsync();

            return true;
        }
    }
}
