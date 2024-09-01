using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Models.DTOModels
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class TransactionPostDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
    }
}
