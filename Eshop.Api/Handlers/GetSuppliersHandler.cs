using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;
using Newtonsoft.Json;

namespace Eshop.Api.Handlers
{
    public class GetSuppliersHandler : IRequestHandler<GetSupplliersQuery, IEnumerable<SupplierDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetSuppliersHandler(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<SupplierDTO>> Handle(GetSupplliersQuery request, CancellationToken cancellationToken)
        {
            var suppliers = await uow.SupplierRepository.GetAllAsync(request.RequestParameter,includes:"Categories");
            if (suppliers is null)
                throw new NotFoundException($"No supplier exists in database");

            httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination",JsonConvert.SerializeObject(suppliers.MetaData));
            return mapper.Map<IEnumerable<SupplierDTO>>(suppliers);
        }
    }
}
