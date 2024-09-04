using eshop.DataAccess.Data;
using Eshop.Models;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
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
    public class TransactionPostValidator : AbstractValidator<TransactionPostDTO>
    {
        public TransactionPostValidator()
        {
            RuleFor(x => x.Amount).NotEmpty().NotNull().GreaterThanOrEqualTo(1).WithMessage("Amount must be greater than or equal to 1");
        }
    }
    public class OrderValidator : AbstractValidator<OrderPostDTO>
    {
        public OrderValidator()
        {
            RuleFor(x => x.City).NotNull().NotEmpty().WithMessage("City is required");
            RuleFor(x => x.Address).NotNull().NotEmpty().WithMessage("Address is required");
            RuleFor(x => x.TrackingNumber).NotNull().NotEmpty().WithMessage("TrackingNumber is required");
            RuleFor(x => x.Carrier).NotNull().NotEmpty().WithMessage("Carrier is required");
        }
    }
    public class OrderDetailValidator : AbstractValidator<OrderDetail>
    {
        public OrderDetailValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().NotNull().GreaterThanOrEqualTo(1).WithMessage("OrderId must be greater than or equal to 1");
            RuleFor(x => x.Quantity).NotEmpty().NotNull().GreaterThanOrEqualTo(1).WithMessage("Quantity must be greater than or equal to 1");
            RuleFor(x => x.ProductId).NotEmpty().NotNull().GreaterThanOrEqualTo(1).WithMessage("ProductId must be greater than or equal to 1");
            RuleFor(x => x.UnitPrice).NotEmpty().NotNull().GreaterThanOrEqualTo(1).WithMessage("UnitPrice must be greater than or equal to 1");
        }
    }
    public class RegisterModelValidator : AbstractValidator<RegisterModel>
    {
        public RegisterModelValidator()
        {
            RuleFor(u => u.UserName)
            .NotEmpty().WithMessage("First name is required");

            RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
            RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one numeric digit")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
        }
    }
    public class UserModelDTOValidator : AbstractValidator<UserModelDTO>
    {
        public UserModelDTOValidator()
        {
            RuleFor(u => u.UserName)
            .NotEmpty().WithMessage("UserName is required");

            RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
            RuleFor(u => u.FirstName)
            .NotEmpty().WithMessage("FirstName is required");
            RuleFor(u => u.LastName)
            .NotEmpty().WithMessage("FirstName is required");
            RuleFor(u => u.Phone)
            .NotEmpty().WithMessage("Phone is required");
        }
    }
}
