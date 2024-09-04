using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands;
using Eshop.DataAccess.Services.Middleware;
using Eshop.DataAccess.Services.Validators;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class GetUpdatePatchSupplierHandler:IRequestHandler<UpdatePatchSupplierRequest,bool>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetUpdatePatchSupplierHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(UpdatePatchSupplierRequest request, CancellationToken cancellationToken)
        {
            if (request.supplierId <= 0)
            {
                throw new BadRequestException($"ERROR Invalid model:{request.patch.ToString()} or Id:{request.supplierId} values");
            }
            var existingSupplier = await uow.SupplierRepository.GetByIdAsync(request.supplierId);
            if (existingSupplier is null)
            {
                throw new BadRequestException($"Supplier with Id: {request.supplierId} is not found");
            }
            var dto_supplier = mapper.Map<SupplierDTO>(existingSupplier);
            request.patch.ApplyTo(dto_supplier);

            var validate = new SupplierValidator();
            var result = validate.Validate(dto_supplier);
            if (!result.IsValid)
            {
                throw new BadRequestException($"{string.Join(",",result.Errors)}");
            }

            var supplier = mapper.Map<Supplier>(dto_supplier);
            await uow.SupplierRepository.UpdatePatchAsync(request.supplierId, supplier);
            await uow.CommitAsync();

            return true;
        }
    }
}
