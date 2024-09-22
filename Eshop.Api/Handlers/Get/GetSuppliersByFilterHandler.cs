using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries.Supplier;
using Eshop.DataAccess.DataShaping;
using Eshop.DataAccess.Services.Links;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Handlers.Get
{
    public class GetSuppliersByFilterHandler : IRequestHandler<GetSuppliersByFilterQuery, IEnumerable<SupplierDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IDataShaper<SupplierDTO> dataShaper;
        private readonly ILinksService linksService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetSuppliersByFilterHandler(IUnitOfWork uow, IMapper mapper, IDataShaper<SupplierDTO> dataShaper, IHttpContextAccessor httpContextAccessor, ILinksService linksService)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.dataShaper = dataShaper;
            this.linksService = linksService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<SupplierDTO>> Handle(GetSuppliersByFilterQuery request, CancellationToken cancellationToken)
        {
            var suppliers = await uow.SupplierRepository.GetAllByFilterAsync(request.Param);
            if (suppliers is null)
                throw new NotFoundException("Supplier does'nt exists in the database");

            var suppliersDto = mapper.Map<IEnumerable<SupplierDTO>>(suppliers);
            foreach (var item in suppliersDto)
            {
                AddLinks(item);
            }
            return suppliersDto;

        }
        private void AddLinks(SupplierDTO supplier)
        {
            supplier.Links.Add(
                linksService.Generate("GetSupplier", new { supplierId = supplier.Id }, "self", "GET"));
            supplier.Links.Add(
               linksService.Generate("UpdateSupplier", new { supplierId = supplier.Id }, "update-supplier", "PUT"));
            supplier.Links.Add(
               linksService.Generate("DeleteSupplier", new { supplierId = supplier.Id }, "delete-supplier", "DELETE"));

            supplier.Links.Add(
               linksService.Generate("UpdatePatchSupplier", new { supplierId = supplier.Id }, "updatePatch-supplier", "Patch"));
        }
    }
}
