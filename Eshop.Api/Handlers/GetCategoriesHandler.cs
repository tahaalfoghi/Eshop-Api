using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;
using Newtonsoft.Json;

namespace Eshop.Api.Handlers
{
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetCategoriesHandler(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<CategoryDTO>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categroies = await uow.CategoryRepository.GetAllAsync(request.requestParameter,includes:"Supplier");
            if (categroies is null || categroies.Count() == 0)
                throw new NotFoundException("No category exists in database");

            //httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(categroies.MetaData));
            return mapper.Map<IEnumerable<CategoryDTO>>(categroies);
        }
    }
}
