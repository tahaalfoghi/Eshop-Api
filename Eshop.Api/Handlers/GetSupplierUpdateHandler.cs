using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class GetSupplierUpdateHandler : IRequestHandler<UpdateSupplierRequest, bool>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetSupplierUpdateHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(UpdateSupplierRequest request, CancellationToken cancellationToken)
        {
            var result = mapper.Map<Supplier>(request.Supplier);

            await uow.SupplierRepository.UpdateAsync(result);
            await uow.CommitAsync();

            return true;
        }
    }
}
