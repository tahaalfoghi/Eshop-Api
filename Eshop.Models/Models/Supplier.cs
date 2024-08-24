using System.ComponentModel.DataAnnotations;

namespace Eshop.Models.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CompanyName { get; set; } = string.Empty;
        public string? ContactName { get; set; } = string.Empty;
        public string? Country { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public virtual ICollection<Category> Categories { get; set; } =  new List<Category>();
    }
}