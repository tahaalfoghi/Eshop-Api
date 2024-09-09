using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class GetSuppliersByFilterHandler:IRequestHandler<GetSuppliersByFilterQuery,IEnumerable<SupplierDTO>>
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
            var suppliers = await uow.SupplierRepository.GetAllByFilterAsync(request.Param);
            if (suppliers is null)
                throw new NotFoundException("Supplier does'nt exists in the database");

            return mapper.Map<IEnumerable<SupplierDTO>>(suppliers);
        }
    }
}
