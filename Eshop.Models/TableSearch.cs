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
    }
}
