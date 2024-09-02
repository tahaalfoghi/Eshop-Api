using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands;
using Eshop.DataAccess.Services.Middleware;
using Eshop.Models.DTOModels;
using MediatR;
using NuGet.Protocol.Plugins;

namespace Eshop.Api.Handlers
{
    public class GetDeleteCategoryHandler:IRequestHandler<DeleteCategoryRequest,bool>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetDeleteCategoryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
        {
            if (request.CategoryId <= 0)
                throw new BadRequestException($"invalid id [{request.CategoryId}]");

            var category = await uow.CategoryRepository.GetByIdAsync(request.CategoryId);
            if (category is null)
                throw new NotFoundException($"Category:[{request.CategoryId}] not exists");

            uow.CategoryRepository.Delete(category);
            await uow.CommitAsync();

            return true;
        }
    }
}
