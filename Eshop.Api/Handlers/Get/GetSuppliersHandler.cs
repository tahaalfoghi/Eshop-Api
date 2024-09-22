using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries.Supplier;
using Eshop.DataAccess.Services.Links;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;
using Newtonsoft.Json;

namespace Eshop.Api.Handlers.Get
{
    public class GetSuppliersHandler : IRequestHandler<GetSupplliersQuery, IEnumerable<SupplierDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILinksService linksService;

        public GetSuppliersHandler(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILinksService linksService)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.linksService = linksService;
        }

        public async Task<IEnumerable<SupplierDTO>> Handle(GetSupplliersQuery request, CancellationToken cancellationToken)
        {
            var suppliers = await uow.SupplierRepository.GetAllAsync(request.RequestParameter, includes: "Categories");
            if (suppliers is null)
                throw new NotFoundException($"No supplier exists in database");

            httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(suppliers.MetaData));
            
            var suppliersDto = mapper.Map<IEnumerable<SupplierDTO>>(suppliers);
            foreach(var item in suppliersDto)
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
