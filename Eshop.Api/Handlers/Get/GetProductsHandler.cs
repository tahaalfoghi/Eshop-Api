using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries.Product;
using Eshop.DataAccess.Services.Links;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;
using Newtonsoft.Json;

namespace Eshop.Api.Handlers.Get
{
    public class GetProductsHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILinksService linksService;
        public GetProductsHandler(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILinksService linksService)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.linksService = linksService;
        }

        public async Task<IEnumerable<ProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await uow.ProductRepository.GetAllAsync(request.requestParameter, includes: "Category");
            if (products is null)
                throw new NotFoundException();

            httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(products.MetaData));

            var productsDto = mapper.Map<IEnumerable<ProductDTO>>(products);
            foreach (var product in productsDto)
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
