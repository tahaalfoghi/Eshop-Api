using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Services.Requests
{
    public class SupplierRequestParamater:RequestParameter
    {
        public string? Country { get; set; }
        public string? CompanyName { get; set; }
        public string? SearchTerm { get; set; }
    }
    public class CategoryRequestParamater : RequestParameter
    {
        public string? SupplierName { get; set; }
        public string? Name { get; set; }
    }
    public class ProductRequestParamater : RequestParameter
    {
        public string? CategoryName { get; set; }
        public decimal Price { get; set; }
        public bool ValidPrice => Price > 0;
    }
    public class TransactionRequestParamater : RequestParameter
    {

    }
    public class OrderRequestParamater : RequestParameter
    {
        public string? UserName { get; set; }
        public DateTime Date { get; set; }
    }
    public class OrderDetailRequestParamater : RequestParameter
    {
        
    }
    public class CartRequestParamater : RequestParameter
    {

    }
}
