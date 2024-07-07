using AutoMapper;
using BusinessLogicLayer.Dtos.Orders;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Profiles
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<OrderCreateDto, Order>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<Order, OrderReadDto>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
        }
    }
}
