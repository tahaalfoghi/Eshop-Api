using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands;
using Eshop.DataAccess.Services.Middleware;
using Eshop.DataAccess.Services.Validators;
using Eshop.Models.Models;
using MediatR;

namespace Eshop.Api.Handlers.Update
{
    public class GetUpdateTransactionHandler : IRequestHandler<UpdateTransacrtionRequest, bool>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetUpdateTransactionHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(UpdateTransacrtionRequest request, CancellationToken cancellationToken)
        {
            var validate = new TransactionPostValidator();
            var result = validate.Validate(request.TransactionDto);
            if (!result.IsValid)
                throw new InvalidModelException($"{string.Join(",", result.Errors.ToString())}");

            var transInDb = mapper.Map<Transaction>(request.TransactionDto);
            if (transInDb is null)
                throw new NotFoundException($"Transaction [{request.TransactionDto.Id}] not found");

            uow.TransactionRepository.Update(transInDb);
            await uow.CommitAsync();

            return true;
        }
    }
}
