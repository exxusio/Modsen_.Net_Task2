using BusinessLogicLayer;
using BusinessLogicLayer.Dtos.Users;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = $"{RoleConstants.Admin}")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken = default)
        {
            var users = await _userService.GetAllUsersAsync(cancellationToken);
            return Ok(users);
        }

        [Authorize(Roles = $"{RoleConstants.Admin}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _userService.GetUserByIdAsync(id, cancellationToken);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateDto userCreateDto, CancellationToken cancellationToken = default)
        {
            var user = await _userService.CreateUserAsync(userCreateDto, cancellationToken);
            return Ok(user);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UserUpdateDto userUpdateDto, CancellationToken cancellationToken = default)
        {
            userUpdateDto.Id = id;
            var user = await _userService.UpdateUserAsync(userUpdateDto, cancellationToken);
            return Ok(user);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken = default)
        {
            var deletedUser = await _userService.DeleteUserByIdAsync(id, cancellationToken);
            return Ok(deletedUser);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(UserLoginDto loginDto, CancellationToken cancellationToken = default)
        {
            var tokenResponse = await _userService.AuthenticateAsync(loginDto, cancellationToken);
            return Ok(tokenResponse);
        }
    }
}
