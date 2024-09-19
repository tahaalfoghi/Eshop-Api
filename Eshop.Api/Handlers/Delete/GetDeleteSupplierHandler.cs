using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands.Delete;
using Eshop.DataAccess.Services.Middleware;
using MediatR;

namespace Eshop.Api.Handlers.Delete
{
    public class GetDeleteSupplierHandler : IRequestHandler<DeleteSupplierRequest, bool>
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
            if (request.SupplierId <= 0)
                throw new BadRequestException($"Invalid id: {request.SupplierId}");

            var supplier = await uow.SupplierRepository.GetByIdAsync(request.SupplierId);
            if (supplier is null)
                throw new NotFoundException($"Supplier [{request.SupplierId}] doesn't exits");

            uow.SupplierRepository.Delete(supplier);
            await uow.CommitAsync();

            return true;
        }
    }
}
