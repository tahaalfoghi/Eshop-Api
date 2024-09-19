using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands.Create;
using Eshop.DataAccess.Services.Middleware;
using Eshop.DataAccess.Services.Validators;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using MediatR;

namespace Eshop.Api.Handlers.Create
{
    public class GetCreateSupplierHandle : IRequestHandler<CreateSupplierRequest, SupplierDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetCreateSupplierHandle(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<SupplierDTO> Handle(CreateSupplierRequest request, CancellationToken cancellationToken)
        {
            var validate = new SupplierValidator();
            var result = validate.Validate(request.Supplier);
            if (!result.IsValid)
                throw new InvalidModelException($"{result.Errors.ToString()}");

            var supplier = mapper.Map<Supplier>(request.Supplier);
            await uow.SupplierRepository.CreateAsync(supplier);
            await uow.CommitAsync();

            return mapper.Map<SupplierDTO>(supplier);
        }
    }
}
