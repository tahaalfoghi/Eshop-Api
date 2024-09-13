using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Models.DTOModels
{
    public class SupplierDTO
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string? ContactName { get; set; } = string.Empty;
        public string? Country { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public List<Link> Links { get; set; } = new();
        
    }
    public record Link(string Href, string Rel, string Method);
}
