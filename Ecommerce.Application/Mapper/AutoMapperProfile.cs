using AutoMapper;
using Ecommerce.Context;
using Ecommerce.Dtos.Account;
using Ecommerce.Dtos.Categories;
using Ecommerce.Dtos.Product;
using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Mapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, CreateOrUpdateProductDTO>().ReverseMap();
            CreateMap<Product, GetAllProductDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
           // CreateMap<RegisterDTO, EcommerceUser>.ReverseMap();

        }
    }
}


