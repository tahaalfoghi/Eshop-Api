using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Eshop.Api.Handlers
{
    public class GetProductsHandler:IRequestHandler<GetProductsQuery,IEnumerable<ProductDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        public GetProductsHandler(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await uow.ProductRepository.GetAllAsync(request.requestParameter, includes:"Category");
            if (products is null)
                throw new NotFoundException();

            httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(products.MetaData));

            return mapper.Map<IEnumerable<ProductDTO>>(products);
        }
    }
}
