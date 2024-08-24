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
        public string? ASC { get; set; }
        public string? DESC { get; set; }
        public string? Category { get; set; }

        public override string ToString()
        {
            return $"Name:{Name?? "N/A"}  ASC:{ASC?? "N/A"} DESC:{DESC?? "N/A"} Category:{Category?? "N/A"}";

        }
    }
}
