using BusinessLogicLayer.Dtos.Users;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserReadDto>> GetAllUsersAsync(CancellationToken cancellationToken = default);
        Task<UserReadDto> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<UserReadDto> CreateUserAsync(UserCreateDto UserCreateDto, CancellationToken cancellationToken = default);
        Task<UserReadDto> UpdateUserAsync(UserUpdateDto UserUpdateDto, CancellationToken cancellationToken = default);
        Task<UserReadDto> DeleteUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<AuthenticationResponseDto> AuthenticateAsync(UserLoginDto loginDto, CancellationToken cancellationToken = default);
    }
}
