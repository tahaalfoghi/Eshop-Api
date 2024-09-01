using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class GetDeleteSupplierHandler : IRequestHandler<DeleteSupplierRequest,bool>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetDeleteSupplierHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(DeleteSupplierRequest request, CancellationToken cancellationToken)
        {
            var supplier = await uow.SupplierRepository.GetByIdAsync(request.SupplierId);
            uow.SupplierRepository.Delete(supplier);
            await uow.CommitAsync();

            return true;
        }
    }
}
