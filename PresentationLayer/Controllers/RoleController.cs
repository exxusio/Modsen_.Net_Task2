using BusinessLogicLayer;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Route("api/v1/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService, CancellationToken cancellationToken = default)
        {
            _roleService = roleService;
        }

        [Authorize(Roles = $"{RoleConstants.Admin}")]
        [HttpGet]
        public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken = default)
        {
            var roles = await _roleService.GetAllRolesAsync(cancellationToken);
            return Ok(roles);
        }

        [Authorize(Roles = $"{RoleConstants.Admin}")]
        [HttpGet("{role}/users")]
        public async Task<IActionResult> GetUsersWithRole(string role, CancellationToken cancellationToken = default)
        {
            var users = await _roleService.GetUsersWithRole(role, cancellationToken);
            return Ok(users);
        }
    }
}
