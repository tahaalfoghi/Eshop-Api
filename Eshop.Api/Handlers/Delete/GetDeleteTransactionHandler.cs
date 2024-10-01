using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands.Delete;
using Eshop.DataAccess.Services.Middleware;
using MediatR;

namespace Eshop.Api.Handlers.Delete
{
    public class GetDeleteTransactionHandler : IRequestHandler<DeletePaymentRequest, bool>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetDeleteTransactionHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(DeletePaymentRequest request, CancellationToken cancellationToken)
        {
            if (request.TransactionId <= 0)
                throw new BadRequestException($"Invalid id:{request.TransactionId}");

            var transaction = await uow.TransactionRepository.GetByIdAsync(request.TransactionId);
            if (transaction is null)
                throw new NotFoundException($"transaction [{request.TransactionId}] not exists");

            uow.TransactionRepository.Delete(transaction);
            await uow.CommitAsync();

            return true;
        }
    }
}
