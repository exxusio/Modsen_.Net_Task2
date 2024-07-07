using AutoMapper;
using BusinessLogicLayer.Dtos.Categories;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Profiles
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<CategoryCreateDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore());

            CreateMap<CategoryUpdateDto, Category>()
                .ForMember(dest => dest.Products, opt => opt.Ignore());

            CreateMap<Category, CategoryReadDto>();

            CreateMap<Category, CategoryDetailedReadDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
        }
    }
}