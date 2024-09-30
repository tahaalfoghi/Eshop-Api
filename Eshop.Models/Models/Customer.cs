using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Models.Models
{
    public class Customer:ApplicationUser
    {
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string Address { get;set; } = string.Empty;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
