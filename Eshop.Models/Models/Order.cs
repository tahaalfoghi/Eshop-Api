using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Models.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string CustomerId { get; set; } = string.Empty;
        [ForeignKey("CustomerId")]
        public virtual ApplicationUser ApplicationUser { get; set; } = default!;
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Carrier { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string TrackingNumber { get; set; } = string.Empty;
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
