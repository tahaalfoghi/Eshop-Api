using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Links;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Handlers.Get
{
    public class GetProductsByFilterHandler : IRequestHandler<GetProductsByFilterQuery, IEnumerable<ProductDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly ILinksService linksService;
        private readonly IHttpContextAccessor httpContextAccessor;
        public GetProductsByFilterHandler(IUnitOfWork uow, IMapper mapper, ILinksService linksService, IHttpContextAccessor httpContextAccessor)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.linksService = linksService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ProductDTO>> Handle(GetProductsByFilterQuery request, CancellationToken cancellationToken)
        {
            var products = await uow.ProductRepository.GetAllByFilterAsync(request.Param, includes: "Category");
            if (products is null)
                throw new NotFoundException($"Product doesn't exits");
            
            var productsDto = mapper.Map<IEnumerable<ProductDTO>>(products);
            foreach(var product in productsDto)
            {
                AddLinks(product);
            }
            return productsDto;
        }
        private void AddLinks(ProductDTO product)
        {
            product.Links.Add(
                linksService.Generate("GetProduct", new { productId = product.Id }, "self", "GET"));
            product.Links.Add(
               linksService.Generate("UpdateProduct", new { productId = product.Id }, "update-product", "PUT"));
            product.Links.Add(
               linksService.Generate("DeleteProduct", new { productId = product.Id }, "delete-product", "DELETE"));
            product.Links.Add(
               linksService.Generate("UpdatePatchProduct", new { productId = product.Id }, "updatePatch-product", "Patch"));
        }
    }
}
