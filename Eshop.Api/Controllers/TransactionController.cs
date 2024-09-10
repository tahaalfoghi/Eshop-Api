using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands;
using Eshop.Api.Handlers;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Requests;
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

        private readonly IMediator mediator;

        public TransactionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("transactions")]
        public async Task<IActionResult> GetTransactions([FromQuery] RequestParameter requestParameter)
        {
            var query = new GetTransactionsQuery(requestParameter);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpGet]
        [Route("transactions/{transactionId:int}")]
        public async Task<IActionResult> GetTransactionById(int transactionId)
        {
            var query = new GetTransactionQuery(transactionId);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpPut]
        [Route("edit-transaction")]
        public async Task<IActionResult> UpdateTransaction([FromForm] TransactionPostDTO dto_trans)
        {
            var command = new UpdateTransacrtionRequest(dto_trans);
            var result = await mediator.Send(command);
            return Ok($"transaction {dto_trans.Id} updated successfully");

        }
        [HttpDelete]
        [Route("delete-transaction/{Id:int}")]
        public async Task<IActionResult> DeleteTransaction(int Id)
        {
            var command = new DeleteTransactionRequest(Id);
            var result = await mediator.Send(command);
            return Ok($"transaction {Id} deleted successfully");
        }
       
    }
}
