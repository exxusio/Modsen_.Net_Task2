using BusinessLogicLayer.Dtos.Roles;
using BusinessLogicLayer.Dtos.Users;

namespace Tests.Services
{
    public class RoleServiceTests
    {
        private readonly Mock<IRoleRepository> _mockRoleRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly IRoleService _roleService;
        private readonly CancellationToken cancellationToken;

        public RoleServiceTests()
        {
            _mockRoleRepository = new Mock<IRoleRepository>();
            _mockMapper = new Mock<IMapper>();
            _roleService = new RoleService(_mockRoleRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllRolesAsync_ReturnsListOfRoleReadDto()
        {
            var roles = new List<Role>
            {
                new Role { Id = Guid.NewGuid(), Name = "Admin" },
                new Role { Id = Guid.NewGuid(), Name = "User" }
            };
            var expectedReadDtos = new List<RoleReadDto>
            {
                new RoleReadDto { Name = roles[0].Name },
                new RoleReadDto { Name = roles[1].Name }
            };

            _mockRoleRepository.Setup(repo => repo.GetAllAsync(cancellationToken)).ReturnsAsync(roles);
            _mockMapper.Setup(m => m.Map<IEnumerable<RoleReadDto>>(roles)).Returns(expectedReadDtos);

            var result = await _roleService.GetAllRolesAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().ContainItemsAssignableTo<RoleReadDto>();
        }

        [Fact]
        public async Task GetRoleByIdAsync_ExistingId_ReturnsRoleReadDto()
        {
            var roleId = Guid.NewGuid();
            var role = new Role { Id = roleId, Name = "Admin" };
            var expectedReadDto = new RoleReadDto { Name = role.Name };

            _mockRoleRepository.Setup(repo => repo.GetByIdAsync(roleId, cancellationToken)).ReturnsAsync(role);
            _mockMapper.Setup(m => m.Map<RoleReadDto>(role)).Returns(expectedReadDto);

            var result = await _roleService.GetRoleByIdAsync(roleId);

            result.Should().NotBeNull();
            result.Name.Should().Be("Admin");
        }

        [Fact]
        public async Task GetRoleByIdAsync_NonExistingId_ThrowsNotFoundException()
        {
            var roleId = Guid.NewGuid();
            _mockRoleRepository.Setup(repo => repo.GetByIdAsync(roleId, cancellationToken)).ReturnsAsync((Role)null);

            Func<Task> action = async () => await _roleService.GetRoleByIdAsync(roleId);

            await action.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetUsersWithRole_ExistingRoleName_ReturnsListOfUserReadDto()
        {
            var roleName = "Admin";
            var roleId = Guid.NewGuid();
            var role = new Role { Id = roleId, Name = roleName };

            var users = new List<User>
            {
                new User { Id = Guid.NewGuid(), UserName = "user1", HashedPassword = "pwd1", RoleId = roleId, Role = role },
                new User { Id = Guid.NewGuid(), UserName = "user2", HashedPassword = "pwd2", RoleId = roleId, Role = role }
            };

            var expectedReadDtos = users.Select(u => new UserReadDto
            {
                UserName = u.UserName,
                Role = new RoleReadDto { Name = u.Role.Name }
            }).ToList();

            _mockRoleRepository.Setup(repo => repo.GetByPredicateAsync(r => r.Name == roleName, cancellationToken))
                               .ReturnsAsync(new List<Role> { role });

            _mockMapper.Setup(m => m.Map<IEnumerable<UserReadDto>>(It.IsAny<IEnumerable<User>>()))
                       .Returns(expectedReadDtos);

            var result = await _roleService.GetUsersWithRole(roleName);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().ContainItemsAssignableTo<UserReadDto>();
        }

        [Fact]
        public async Task GetUsersWithRole_NonExistingRoleName_ThrowsNotFoundException()
        {
            var roleName = "NonExistingRole";
            _mockRoleRepository.Setup(repo => repo.GetByPredicateAsync(r => r.Name == roleName, cancellationToken)).ReturnsAsync(new List<Role>());

            Func<Task> action = async () => await _roleService.GetUsersWithRole(roleName);

            await action.Should().ThrowAsync<NotFoundException>();
        }
    }
}