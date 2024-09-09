using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class GetCategoriesByFilterHandler:IRequestHandler<GetCategoriesByFilterQuery,IEnumerable<CategoryDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetCategoriesByFilterHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDTO>> Handle(GetCategoriesByFilterQuery request, CancellationToken cancellationToken)
        {
            var categories = await uow.CategoryRepository.GetAllByFilterAsync(request.Params, includes: "Supplier");
            if (categories is null || categories.Count() == 0)
                throw new NotFoundException($"no category exists with this filter:{request.Params.ToString()}");

            return mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }
    }
}
