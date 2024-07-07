using AutoMapper;
using BusinessLogicLayer.Dtos.Users;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Profiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserCreateDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RoleId, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.HashedPassword, opt => opt.Ignore());

            CreateMap<UserUpdateDto, User>()
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.HashedPassword, opt => opt.Ignore());

            CreateMap<User, UserReadDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));
        }
    }
}
