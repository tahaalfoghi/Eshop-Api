using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands.Create;
using Eshop.DataAccess.Services.Middleware;
using Eshop.DataAccess.Services.Validators;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using MediatR;

namespace Eshop.Api.Handlers.Create
{
    public class GetCreateCategoryHandler : IRequestHandler<CreateCategoryRequest, CategoryDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetCreateCategoryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<CategoryDTO> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            var validate = new CategoryValidator();
            var result = validate.Validate(request.CategoryPostDto);
            if (!result.IsValid)
                throw new InvalidModelException(result.Errors.ToString());

            var category = mapper.Map<Category>(request.CategoryPostDto);
            await uow.CategoryRepository.CreateAsync(category);
            await uow.CommitAsync();

            return mapper.Map<CategoryDTO>(category);
        }
    }
}
