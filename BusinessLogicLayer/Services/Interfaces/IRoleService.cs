using BusinessLogicLayer.Dtos.Roles;
using BusinessLogicLayer.Dtos.Users;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleReadDto>> GetAllRolesAsync(CancellationToken cancellationToken = default);
        Task<RoleReadDto> GetRoleByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserReadDto>> GetUsersWithRole(string roleName, CancellationToken cancellationToken = default);
    }
}
