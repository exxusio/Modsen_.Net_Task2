using AutoMapper;
using BusinessLogicLayer.Dtos.OrderItems;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Profiles
{
    public class OrderItemMappingProfile : Profile
    {
        public OrderItemMappingProfile()
        {
            CreateMap<OrderItemCreateDto, OrderItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            CreateMap<OrderItem, OrderItemReadDto>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));

            CreateMap<OrderItemCreateNewOrderDto, OrderItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore());
        }
    }
}
