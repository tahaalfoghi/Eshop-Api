using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Models.DTOModels;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers
{
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
        [Route("GetTransactions")]
        public async Task<IActionResult> GetTransactions()
        {
            var transactions = await uow.TransactionRepository.GetAllAsync(includes:"ApplicationUser");
            if(transactions is null)
                return NotFound($"No transactions found");

           var dto_transactions = mapper.Map<List<TransactionDTO>>(transactions);
           logger.LogInformation($"Get all transactions");
            
            return Ok(dto_transactions);
        }
    }
}
