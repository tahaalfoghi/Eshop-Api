using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class GetProductsHandler:IRequestHandler<GetProductsQuery,IEnumerable<ProductDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetProductsHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await uow.ProductRepository.GetAllAsync(request.requestParameter, includes:"Category");
            if (products is null)
                throw new NotFoundException();

            return mapper.Map<IEnumerable<ProductDTO>>(products);
        }
    }
}
