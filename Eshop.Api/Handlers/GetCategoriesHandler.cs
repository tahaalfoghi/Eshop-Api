using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetCategoriesHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDTO>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categroies = await uow.CategoryRepository.GetAllAsync(includes:"Supplier");
            if (categroies is null || categroies.Count() == 0)
                throw new NotFoundException("No category exists in database");

            return mapper.Map<IEnumerable<CategoryDTO>>(categroies);
        }
    }
}
