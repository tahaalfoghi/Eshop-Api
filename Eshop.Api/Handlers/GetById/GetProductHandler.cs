using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;
using NuGet.Protocol.Plugins;

namespace Eshop.Api.Handlers.GetById
{
    public class GetProductHandler : IRequestHandler<GetProductQuery, ProductDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetProductHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<ProductDTO> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            if (request.ProductId <= 0)
                throw new BadRequestException($"Invalid id value:{request.ProductId}");

            var product = await uow.ProductRepository.GetByIdAsync(request.ProductId, "Category");
            if (product is null)
                throw new NotFoundException($"Product with id:{request.ProductId} doesn't exists");

            return mapper.Map<ProductDTO>(product);
        }
    }
}
