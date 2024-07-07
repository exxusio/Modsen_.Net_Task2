using AutoMapper;
using BusinessLogicLayer.Dtos.Products;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Profiles
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<ProductCreateDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<ProductUpdateDto, Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<Product, ProductReadDto>();

            CreateMap<Product, ProductDetailedReadDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
        }
    }
}
