using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class GetCategoryHandler:IRequestHandler<GetCategoryQuery,CategoryDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetCategoryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<CategoryDTO> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await uow.CategoryRepository.GetByIdAsync(request.CategoryId,includes:"Supplier");
            if (category is null)
                throw new NotFoundException($"Category [{request.CategoryId}] not exist");

            var dtoCategory = mapper.Map<CategoryDTO>(category);
            return dtoCategory;
        }
    }
}
