using AutoMapper;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.Product;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.ProductCategory;
using E_commerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Create
            CreateMap<CreateCategoryDto, Category>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
            CreateMap<Category, CreateCategoryDto>();

            CreateMap<CreateProductDto, Product>()
              .ForMember(dest => dest.UserId, opt => opt.Ignore())
              .ForMember(dest => dest.User, opt => opt.Ignore());
            CreateMap<Product, CreateProductDto>();

            // Read
          
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, GetCategoryDto>().ReverseMap();

            CreateMap<Product, GetProductDto>().
                ForMember(dest => dest.DisplayPrice, opt => opt.MapFrom(src => $"{src.Currency} {src.UnitPrice:N2}"));

            CreateMap<Product, ProductSummaryDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();

         
            // Update
            CreateMap<UpdateCategoryDto, Category>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
            CreateMap<Category, UpdateCategoryDto>();



            CreateMap<UpdateProductDto, Product>()
              .ForMember(dest => dest.UserId, opt => opt.Ignore())
              .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<Product, UpdateProductDto>();



        }
    }
}
