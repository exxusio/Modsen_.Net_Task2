using BusinessLogicLayer.Dtos.Roles;

namespace BusinessLogicLayer.Dtos.Users
{
    public class UserReadDto
    {
        public string UserName { get; set; }
        public RoleReadDto Role { get; set; }
    }
}
