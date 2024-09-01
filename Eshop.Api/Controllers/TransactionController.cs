using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.DataAccess.Services.Validators;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController:ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly ILogger<TransactionController> logger;

        public TransactionController(IUnitOfWork uow, IMapper mapper, ILogger<TransactionController> logger)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        [Route("Transactions")]
        public async Task<IActionResult> GetTransactions()
        {
            var transactions = await uow.TransactionRepository.GetAllAsync(includes:"ApplicationUser");
            if(transactions is null)
                return NotFound($"No transactions found");

           var dto_transactions = mapper.Map<List<TransactionDTO>>(transactions);
           logger.LogInformation($"Get all transactions");
            
            return Ok(dto_transactions);
        }
        [HttpPut]
        [Route("EditTransaction/{Id:int}")]
        public async Task<IActionResult> UpdateTransaction(int Id,[FromForm] TransactionPostDTO dto_trans)
        {
            var trans = await uow.TransactionRepository.GetByIdAsync(Id, includes: "Order,ApplicationUser");
            if (trans is null)
                return NotFound($"Transaction with {Id} not found");

            var validate = new TransactionPostValidator();
            var result = validate.Validate(dto_trans);
            if (!result.IsValid)
            {
                logger.LogError($"Invalid model for update transaction");
                return BadRequest($"Error: {result.Errors.ToString()}");
            }

            trans= mapper.Map<Transaction>(dto_trans);
            uow.TransactionRepository.Update(trans);
            await uow.CommitAsync();

            return Ok($"Transaction updated successfully");
        }
        [HttpDelete]
        [Route("DeleteTransaction/{Id:int}")]
        public async Task<IActionResult> DeleteTransaction(int Id)
        {
            if (Id <= 0)
                return BadRequest($"Invalid id value");

            var trans = await uow.TransactionRepository.GetByIdAsync(Id);
            if (trans is null)
                return NotFound($"Transaction {Id} not found");

            uow.TransactionRepository.Delete(trans);
            await uow.CommitAsync();

            return Ok($"Transaction deleted");
        }
    }
}
