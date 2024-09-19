using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands.Update;
using Eshop.DataAccess.Services.Middleware;
using Eshop.DataAccess.Services.Validators;
using Eshop.Models.Models;
using MediatR;

namespace Eshop.Api.Handlers.Update
{
    public class GetUpdateCategoryHandler : IRequestHandler<UpdateCategoryRequest, bool>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetUpdateCategoryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            var validate = new CategoryValidator();
            var result = validate.Validate(request.CategoryDto);
            if (!result.IsValid)
                throw new InvalidModelException($"{result.Errors.ToString()}");


            var category = mapper.Map<Category>(request.CategoryDto);
            if (await uow.CategoryRepository.GetByIdAsync(category.Id) is null)
                throw new NotFoundException($"Category [{category.Id}] not exits");

            await uow.CategoryRepository.UpdateAsync(category);
            await uow.CommitAsync();

            return true;
        }
    }
}
