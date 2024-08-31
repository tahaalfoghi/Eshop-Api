using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Models
{
    public class TableSearch
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public SortBy Sort { get; set; }
        public string? Category { get; set; }
        public string? UserId { get; set; }
        public override string ToString()
        {
            return $"Name:{Name?? "N/A"} Category:{Category?? "N/A"}";

        }
    }
    public enum SortBy
    {
        Asc,
        Desc
    }
}
