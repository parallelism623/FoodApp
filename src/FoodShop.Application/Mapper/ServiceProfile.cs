using AutoMapper;
using FoodShop.Contract.DataTransferObjects.Request.V1;
using FoodShop.Contract.DataTransferObjects.Response.V1;
using FoodShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Mapper
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<Product, ProductResponseList>().ReverseMap();
            CreateMap<Product, ProductResponse>().ReverseMap();
            CreateMap<Product, CreateProductRequest>().ReverseMap();
            CreateMap<Product, UpdateProductRequest>().ReverseMap();
        }
    }
}
