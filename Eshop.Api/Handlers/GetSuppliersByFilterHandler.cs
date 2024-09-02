using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class GetSuppliersByFilterHandler:IRequestHandler<GetSuppliersByFilterQuery, IEnumerable<SupplierDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetSuppliersByFilterHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<SupplierDTO>> Handle(GetSuppliersByFilterQuery request, CancellationToken cancellationToken)
        {
            var suppliers = await uow.SupplierRepository.GetAllByFilterAsync(request.Search);
            if (suppliers is null || suppliers.Count() == 0)
                throw new NotFoundException($"No suppliers exists with this filter: [{request.Search}]");

            var dtoSuppliers = mapper.Map<IEnumerable<SupplierDTO>>(suppliers);
            return dtoSuppliers;
        }
    }
    public class GetSupplierByFilterHandler : IRequestHandler<GetSupplierByFilterQuery, SupplierDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetSupplierByFilterHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<SupplierDTO> Handle(GetSupplierByFilterQuery request, CancellationToken cancellationToken)
        {
            var supplier = await uow.SupplierRepository.GetFirstOrDefaultAsync(request.Search);
            if (supplier is null )
                throw new NotFoundException($"No suppliers exists with this filter: [{request.Search}]");

            var dtoSupplier = mapper.Map<SupplierDTO>(supplier);
            return dtoSupplier;
        }
    }
}
