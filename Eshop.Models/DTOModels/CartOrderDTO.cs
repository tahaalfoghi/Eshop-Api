using Eshop.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Models.DTOModels
{
    public class CartOrderDTO
    {
        public IEnumerable<Cart> Carts { get; set; } = new List<Cart>();
        public Order Order { get; set; } = default!;
    }
}
