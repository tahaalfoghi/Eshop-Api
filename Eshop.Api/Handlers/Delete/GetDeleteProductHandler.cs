using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands;
using Eshop.DataAccess.Services.Middleware;
using MediatR;

namespace Eshop.Api.Handlers.Delete
{
    public class GetDeleteProductHandler : IRequestHandler<DeleteProductRequest, bool>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetDeleteProductHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {
            if (request.ProductId <= 0)
                throw new BadRequestException($"Invalid id value:{request.ProductId}");

            var product = await uow.ProductRepository.GetByIdAsync(request.ProductId);
            if (product is null)
                throw new NotFoundException($"Product [{request.ProductId}] not found");

            uow.ProductRepository.Delete(product);
            await uow.CommitAsync();
            return true;
        }
    }
}
