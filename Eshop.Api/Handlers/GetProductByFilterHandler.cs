using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class GetProductByFilterHandler:IRequestHandler<GetProductByFilterQuery,ProductDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetProductByFilterHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<ProductDTO> Handle(GetProductByFilterQuery request, CancellationToken cancellationToken)
        {
            var products = await uow.ProductRepository.GetFirstOrDefaultAsync(request.search,includes:"Category");
            if (products is null)
                throw new NotFoundException($"Product doesn't exits");

            return mapper.Map<ProductDTO>(products);
        }
    }
}
