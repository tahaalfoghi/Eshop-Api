using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries.Product;
using Eshop.DataAccess.Services.Links;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using MediatR;
using NuGet.Protocol.Plugins;

namespace Eshop.Api.Handlers.GetById
{
    public class GetProductHandler : IRequestHandler<GetProductQuery, ProductDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly ILinksService linksService;
        private readonly IHttpContextAccessor httpContextAccessor;
        public GetProductHandler(IUnitOfWork uow, IMapper mapper, ILinksService linksService, IHttpContextAccessor httpContextAccessor)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.linksService = linksService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<ProductDTO> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            if (request.ProductId <= 0)
                throw new BadRequestException($"Invalid id value:{request.ProductId}");

            var product = await uow.ProductRepository.GetByIdAsync(request.ProductId, "Category");
            if (product is null)
                throw new NotFoundException($"Product with id:{request.ProductId} doesn't exists");

            var productDto = mapper.Map<ProductDTO>(product);
            AddLinks(productDto);
            return productDto;
        }
        private void AddLinks(ProductDTO product)
        {
            product.Links.Add(
                linksService.Generate("GetProduct", new { productId =  product.Id }, "self", "GET"));
            product.Links.Add(
               linksService.Generate("UpdateProduct", new { productId = product.Id }, "update-product", "PUT"));
            product.Links.Add(
               linksService.Generate("DeleteProduct", new { productId = product.Id }, "delete-product", "DELETE"));
            product.Links.Add(
               linksService.Generate("UpdatePatchProduct", new { productId = product.Id }, "updatePatch-product", "Patch"));
        }
    }
}
