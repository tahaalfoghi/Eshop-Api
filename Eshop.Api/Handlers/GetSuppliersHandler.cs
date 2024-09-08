using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class GetSuppliersHandler : IRequestHandler<GetSupplliersQuery, IEnumerable<SupplierDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetSuppliersHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<SupplierDTO>> Handle(GetSupplliersQuery request, CancellationToken cancellationToken)
        {
            var suppliers = await uow.SupplierRepository.GetAllAsync(request.RequestParameter,includes:"Categories");
            if (suppliers is null)
                throw new NotFoundException($"No supplier exists in database");

            return mapper.Map<IEnumerable<SupplierDTO>>(suppliers);
        }
    }
}
