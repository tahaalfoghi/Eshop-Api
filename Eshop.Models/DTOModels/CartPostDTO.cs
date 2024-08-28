using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Models.DTOModels
{
    public class CartPostDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
    }
}
