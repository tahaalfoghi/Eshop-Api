using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Models
{
    public class TableSearch
    {
        public int Rows { get; set; }
        public int Skip { get; set; }
        public string? GlobalFilters { get; set; }
        public SortBy Sort { get; set; }
        public override string ToString()
        {
            return $"Rows:{Rows} Skip:{Skip} GlobalFilters:{string.Join(",",GlobalFilters)}";

        }
    }
    public enum SortBy
    {
        Asc,
        Desc
    }
}
