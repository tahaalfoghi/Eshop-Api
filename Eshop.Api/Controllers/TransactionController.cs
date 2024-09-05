using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands;
using Eshop.Api.Handlers;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Validators;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Eshop-UI")]
    public class TransactionController:ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly ILogger<TransactionController> logger;
        private readonly IMediator mediator;

        public TransactionController(IUnitOfWork uow, IMapper mapper, ILogger<TransactionController> logger, IMediator mediator)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.logger = logger;
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("Transactions")]
        public async Task<IActionResult> GetTransactions()
        {
            var query = new GetTransactionsQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpGet]
        [Route("Transactions/{transactionId:int}")]
        public async Task<IActionResult> GetTransactionById(int transactionId)
        {
            var query = new GetTransactionQuery(transactionId);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpPut]
        [Route("EditTransaction")]
        public async Task<IActionResult> UpdateTransaction([FromForm] TransactionPostDTO dto_trans)
        {
            var command = new UpdateTransacrtionRequest(dto_trans);
            var result = await mediator.Send(command);
            return Ok($"transaction {dto_trans.Id} updated successfully");

        }
        [HttpDelete]
        [Route("DeleteTransaction/{Id:int}")]
        public async Task<IActionResult> DeleteTransaction(int Id)
        {
            var command = new DeleteTransactionRequest(Id);
            var result = await mediator.Send(command);
            return Ok($"transaction {Id} deleted successfully");
        }
       
    }
}
