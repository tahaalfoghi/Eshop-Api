using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class GetProductsByFilterHandler:IRequestHandler<GetProductsByFilterQuery,IEnumerable<ProductDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetProductsByFilterHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> Handle(GetProductsByFilterQuery request, CancellationToken cancellationToken)
        {
            var products = await uow.ProductRepository.GetAllByFilterAsync(request.Param,includes:"Category");
            if (products is null)
                throw new NotFoundException($"Product doesn't exits");

            return mapper.Map<IEnumerable<ProductDTO>>(products);
        }
    }
}
