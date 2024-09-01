using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class GetSupplierHandler : IRequestHandler<GetSuppllierQuery, SupplierDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetSupplierHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<SupplierDTO> Handle(GetSuppllierQuery request, CancellationToken cancellationToken)
        {
            if (request.SupplierId <= 0)
                throw new Exception($"Invalid id value:{request.SupplierId}");

            var supplier = await uow.SupplierRepository.GetByIdAsync(request.SupplierId);
            if (supplier is null)
                throw new Exception($"Supplier not found");

            return mapper.Map<SupplierDTO>(supplier);
        }
    }
}
