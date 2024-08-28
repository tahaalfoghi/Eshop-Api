using eshop.DataAccess.Data;
using Eshop.Models.DTOModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Services.Validators
{
    public class CategoryValidator : AbstractValidator<CategoryPostDTO>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name required");
            RuleFor(x => x.SupplierId).GreaterThanOrEqualTo(1).WithMessage("The supplier should in range");
        }
    }
    public class SupplierValidator : AbstractValidator<SupplierDTO>
    {
        public SupplierValidator()
        {
            RuleFor(x => x.CompanyName).NotEmpty().WithMessage("Name required");
        }
    }
    public class ProductValidator : AbstractValidator<ProductPostDTO>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Price).NotEmpty().NotNull().GreaterThanOrEqualTo(1).WithMessage("Price must be greater than or equal to 1");
            RuleFor(x => x.CategoryId).NotEmpty().NotNull().GreaterThanOrEqualTo(1);
            RuleFor(x => x.ImageUrl).NotEmpty().NotNull();
        }
    }
    public class CartPostValidator : AbstractValidator<CartPostDTO>
    {
        public CartPostValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().NotNull().GreaterThanOrEqualTo(1).WithMessage("ProductId must be greater than or equal to 1");
            RuleFor(x => x.Count).NotEmpty().NotNull().GreaterThanOrEqualTo(1).WithMessage("Count must be greater than or equal to 1");
        }
    }

}
