using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Models.DTOModels
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public override string? ToString()
        {
            return $"Name:{Name} Description:{Description} Price:{Price} Category:{CategoryName}";
        }
    }
    public class ProductPostDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public override string? ToString()
        {
            return $"Name:{Name} Description:{Description} Price:{Price} {CategoryId}";
        }
    }
}
