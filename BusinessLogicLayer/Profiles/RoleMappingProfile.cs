using AutoMapper;
using BusinessLogicLayer.Dtos.Roles;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Profiles
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<Role, RoleReadDto>();
        }
    }
}
