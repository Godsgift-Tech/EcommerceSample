using AutoMapper;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.ProductCategory;
using E_commerce.Core.Entities;

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

            // Read
            CreateMap<Category, CategoryDto>().ReverseMap();

            // Update
            CreateMap<UpdateCategoryDto, Category>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
            CreateMap<Category, UpdateCategoryDto>();
        }
    }
}
