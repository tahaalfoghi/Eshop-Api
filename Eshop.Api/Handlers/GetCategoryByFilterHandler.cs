using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class GetCategoryByFilterHandler:IRequestHandler<GetCategoryByFilterQuery,CategoryDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetCategoryByFilterHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<CategoryDTO> Handle(GetCategoryByFilterQuery request, CancellationToken cancellationToken)
        {
            var category = await uow.CategoryRepository.GetFirstOrDefaultAsync(request.Search,includes:"Supplier");
            if (category is null)
                throw new NotFoundException($"no category exists with this fiter:{request.Search.ToString()}");

            return mapper.Map<CategoryDTO>(category);
        }
    }
}
