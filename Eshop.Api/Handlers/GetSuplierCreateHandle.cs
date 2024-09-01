using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class GetSupplierCreateHandle : IRequestHandler<CreateSupplierRequest, SupplierDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetSupplierCreateHandle(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<SupplierDTO> Handle(CreateSupplierRequest request, CancellationToken cancellationToken)
        {
            var supplier = mapper.Map<Supplier>(request.Supplier);
            await uow.SupplierRepository.CreateAsync(supplier);
            await uow.CommitAsync();

            return mapper.Map<SupplierDTO>(supplier);
        }
    }
}
