﻿using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands;
using Eshop.Api.Commands.Delete;
using Eshop.Api.Commands.Update;
using Eshop.Api.Handlers;
using Eshop.Api.Queries;
using Eshop.Api.Queries.Transaction;
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
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Eshop-UI")]
    public class PaymentController:ControllerBase
    {

        private readonly IMediator mediator;

        public PaymentController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("Payments")]
        public async Task<IActionResult> GetPayments([FromQuery] RequestParameter requestParameter)
        {
            var query = new GetPaymentsQuery(requestParameter);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpGet]
        [Route("Payments/{transactionId:int}")]
        public async Task<IActionResult> GetPaymentById(int transactionId)
        {
            var query = new GetPaymentQuery(transactionId);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpPut]
        [Route("EditPayment")]
        public async Task<IActionResult> UpdatePayment([FromForm] PaymentDTO dto_trans)
        {
            var command = new UpdatePaymentRequest(dto_trans);
            var result = await mediator.Send(command);
            return Ok($"transaction {dto_trans.Id} updated successfully");

        }
        [HttpDelete]
        [Route("DeletePayment/{Id:int}")]
        public async Task<IActionResult> DeletePayment(int Id)
        {
            var command = new DeletePaymentRequest(Id);
            var result = await mediator.Send(command);
            return Ok($"transaction {Id} deleted successfully");
        }
       
    }
}
