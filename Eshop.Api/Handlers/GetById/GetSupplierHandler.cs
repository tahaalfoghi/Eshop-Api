using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Links;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eshop.Api.Handlers.GetById
{
    public class GetSupplierHandler : IRequestHandler<GetSuppllierQuery, SupplierDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly ILinksService linksService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetSupplierHandler(IUnitOfWork uow, IMapper mapper, ILinksService linksService, IHttpContextAccessor httpContextAccessor)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.linksService = linksService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<SupplierDTO> Handle(GetSuppllierQuery request, CancellationToken cancellationToken)
        {
            if (request.SupplierId <= 0)
                throw new BadRequestException($"Invalid id value:{request.SupplierId}");

            var supplier = await uow.SupplierRepository.GetByIdAsync(request.SupplierId);
            if (supplier is null)
                throw new NotFoundException($"Supplier [{request.SupplierId}] doesn't exists");

            var supplierDTO = mapper.Map<SupplierDTO>(supplier);
            AddLinks(supplierDTO);

            return supplierDTO;
        }
        private void AddLinks(SupplierDTO supplier)
        {
            supplier.Links.Add(
                linksService.Generate("GetSupplier", new { supplierId = supplier.Id }, "self", "GET"));

            supplier.Links.Add(
               linksService.Generate("UpdateSupplier", new { supplierId = supplier.Id }, "update-supplier", "PUT"));

            supplier.Links.Add(
               linksService.Generate("DeleteSupplier", new { supplierId = supplier.Id }, "delete-supplier", "DELETE"));
        }
    }
}
