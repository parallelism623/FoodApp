using AutoMapper;
using FoodShop.Application.Common.DataTransferObjects.Request.V1;
using FoodShop.Application.Common.DataTransferObjects.Respone.V1;
using FoodShop.Domain.Entities;
using FoodShop.Domain.Entities.Identity;

namespace FoodShop.Application.Common.Mapper
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<FoodShop.Domain.Entities.Product, ProductResponseList>();
            CreateMap<FoodShop.Domain.Entities.Product, ProductResponse>();
            CreateMap<CreateProductRequest, FoodShop.Domain.Entities.Product>();
            CreateMap<UpdateProductRequest, FoodShop.Domain.Entities.Product>();
            CreateMap<AppUser, AuthExternalRequest>().ReverseMap();
            CreateMap<AppUser, LoginRequest>().ReverseMap();
            CreateMap<AppUser, UserAuthResponse>().ReverseMap();
            CreateMap<AppUser, RegisterRequest>().ReverseMap();
            CreateMap<AppUser, UserResponse>().ReverseMap();
            CreateMap<AppUser, UserResponseList>().ReverseMap();
            CreateMap<FoodShop.Domain.Entities.Category, CreateCategoryRequest>().ReverseMap();
            CreateMap<FoodShop.Domain.Entities.Category, UpdateCategoryRequest>().ReverseMap();
            CreateMap<OrderListResponse, Order>().ReverseMap();
            
        }
    }
}
