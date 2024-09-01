using AutoMapper;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Services
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Category, CategoryDTO>()
                     .ForMember(x => x.SupplierName, x => x.MapFrom(x => x.Supplier.CompanyName))
                     .ReverseMap();

            CreateMap<Category, CategoryPostDTO>().ReverseMap();
            CreateMap<Supplier, SupplierDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>()
                      .ForMember(x => x.CategoryName, x => x.MapFrom(x => x.Category.Name))
                      .ReverseMap();
            CreateMap<Product, ProductPostDTO>()
                .ForMember(x => x.ImageUrl, x => x.Ignore());

            CreateMap<Cart, CartDTO>()
            .ForMember(x => x.ProductName, x => x.MapFrom(x => x.Product.Name))
            .ForMember(x => x.Price, x => x.MapFrom(x => x.Product.Price))
            .ForMember(x => x.ImgUrl, x => x.MapFrom(x => x.Product.ImageUrl));

            CreateMap<Cart,CartPostDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>()
            .ForMember(x => x.UserEmail, x => x.MapFrom(x => x.ApplicationUser.Email))
            .ForMember(x => x.Username, x => x.MapFrom(x => x.ApplicationUser.UserName));
            
            CreateMap<Order, OrderPostDTO>().ReverseMap();
            CreateMap<Transaction, TransactionDTO>()
            .ForMember(x => x.UserEmail, x => x.MapFrom(x => x.ApplicationUser.Email))
            .ForMember(x => x.UserName, x => x.MapFrom(x => x.ApplicationUser.UserName));

            CreateMap<Transaction, TransactionPostDTO>();
        }
    }
}