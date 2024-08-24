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
    public class MapperConfig:Profile
    {
        public MapperConfig()
        {
            CreateMap<Category, CategoryDTO>()
                     .ForMember(x => x.SupplierName, x => x.MapFrom(x => x.Supplier.CompanyName))
                     .ReverseMap();

            CreateMap<Category,CategoryPostDTO>().ReverseMap();
            CreateMap<Supplier, SupplierDTO>().ReverseMap();
        }
    }
}
