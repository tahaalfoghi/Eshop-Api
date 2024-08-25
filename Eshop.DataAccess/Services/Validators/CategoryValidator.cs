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
    public class CategoryValidator:AbstractValidator<CategoryPostDTO>
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

}
