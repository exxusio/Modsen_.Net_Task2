using BusinessLogicLayer.Dtos.Roles;
using BusinessLogicLayer.Dtos.Users;
using BusinessLogicLayer.Services.Algorithms;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Linq.Expressions;
using System.Data;

namespace Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IPasswordHasher> _mockPasswordHasher;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly IUserService _userService;
        private readonly CancellationToken cancellationToken;

        public UserServiceTests()
        {
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockConfiguration = new Mock<IConfiguration>();
            _userService = new UserService(_mockPasswordHasher.Object, _mockUnitOfWork.Object, _mockMapper.Object, _mockConfiguration.Object);
            cancellationToken = default;
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsListOfUserReadDto()
        {
            var users = new List<User>
            {
                new User { Id = Guid.NewGuid(), UserName = "user1", Role = new Role { Id = Guid.NewGuid(), Name = "User" } },
                new User { Id = Guid.NewGuid(), UserName = "user2", Role = new Role { Id = Guid.NewGuid(), Name = "Admin" } }
            };

            var mockUserRepository = new Mock<IRepository<User>>();
            _mockUnitOfWork.Setup(uow => uow.GetRepository<User>()).Returns(mockUserRepository.Object);
            mockUserRepository.Setup(repo => repo.GetAllAsync(cancellationToken)).ReturnsAsync(users);
            _mockMapper.Setup(m => m.Map<IEnumerable<UserReadDto>>(users)).Returns(users.Select(u => new UserReadDto { UserName = u.UserName, Role = new RoleReadDto { Name = u.Role.Name } }));

            var result = await _userService.GetAllUsersAsync(cancellationToken);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().UserName.Should().Be("user1");
        }

        [Fact]
        public async Task GetUserByIdAsync_ExistingUserId_ReturnsUserReadDto()
        {
            var userId = Guid.NewGuid();
            var existingUser = new User { Id = userId, UserName = "testuser", Role = new Role { Id = Guid.NewGuid(), Name = "User" } };

            var mockUserRepository = new Mock<IRepository<User>>();
            _mockUnitOfWork.Setup(uow => uow.GetRepository<User>()).Returns(mockUserRepository.Object);
            mockUserRepository.Setup(repo => repo.GetByIdAsync(userId, cancellationToken)).ReturnsAsync(existingUser);
            _mockMapper.Setup(m => m.Map<UserReadDto>(existingUser)).Returns(new UserReadDto { UserName = existingUser.UserName, Role = new RoleReadDto { Name = existingUser.Role.Name } });

            var result = await _userService.GetUserByIdAsync(userId, cancellationToken);

            result.Should().NotBeNull();
            result.UserName.Should().Be("testuser");
        }

        [Fact]
        public async Task GetUserByIdAsync_NonExistingUserId_ThrowsNotFoundException()
        {
            var userId = Guid.NewGuid();
            var mockUserRepository = new Mock<IRepository<User>>();
            _mockUnitOfWork.Setup(uow => uow.GetRepository<User>()).Returns(mockUserRepository.Object);
            mockUserRepository.Setup(repo => repo.GetByIdAsync(userId, cancellationToken)).ReturnsAsync((User)null);

            Func<Task> action = async () => await _userService.GetUserByIdAsync(userId, cancellationToken);

            await action.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateUserAsync_ValidUserUpdateDto_ReturnsUpdatedUserReadDto()
        {
            var userId = Guid.NewGuid();
            var roleId = Guid.NewGuid();
            var userUpdateDto = new UserUpdateDto { Id = userId, UserName = "updateduser", Password = "updatedpassword", RoleId = roleId };
            var existingUser = new User { Id = userId, UserName = "olduser", RoleId = userUpdateDto.RoleId };
            var existingRole = new Role { Id = roleId, Name = "Admin" };

            var mockUserRepository = new Mock<IRepository<User>>();
            var mockRoleRepository = new Mock<IRepository<Role>>();

            _mockUnitOfWork.Setup(uow => uow.GetRepository<User>()).Returns(mockUserRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.GetRepository<Role>()).Returns(mockRoleRepository.Object);

            mockUserRepository.Setup(repo => repo.GetByIdAsync(userId, cancellationToken)).ReturnsAsync(existingUser);
            mockRoleRepository.Setup(repo => repo.GetByIdAsync(roleId, cancellationToken)).ReturnsAsync(existingRole);
            _mockPasswordHasher.Setup(ph => ph.HashPassword(userUpdateDto.Password)).Returns("updatedhashedpassword");

            var updatedUserDto = new UserReadDto
            {
                UserName = userUpdateDto.UserName,
                Role = new RoleReadDto { Name = existingRole.Name }
            };
            _mockMapper.Setup(m => m.Map<UserReadDto>(It.IsAny<User>())).Returns(updatedUserDto);

            var result = await _userService.UpdateUserAsync(userUpdateDto, cancellationToken);

            result.Should().NotBeNull();
            result.UserName.Should().Be("updateduser");
            result.Role.Should().NotBeNull();
            result.Role.Name.Should().Be(existingRole.Name);
        }

        [Fact]
        public async Task UpdateUserAsync_NonExistingUserId_ThrowsNotFoundException()
        {
            var userUpdateDto = new UserUpdateDto { Id = Guid.NewGuid(), UserName = "updateduser", Password = "updatedpassword", RoleId = Guid.NewGuid() };

            var mockUserRepository = new Mock<IRepository<User>>();
            _mockUnitOfWork.Setup(uow => uow.GetRepository<User>()).Returns(mockUserRepository.Object);

            mockUserRepository.Setup(repo => repo.GetByIdAsync(userUpdateDto.Id, cancellationToken)).ReturnsAsync((User)null);

            Func<Task> action = async () => await _userService.UpdateUserAsync(userUpdateDto, cancellationToken);

            await action.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateUserAsync_InvalidRoleId_ThrowsNotFoundException()
        {
            var userUpdateDto = new UserUpdateDto { Id = Guid.NewGuid(), UserName = "updateduser", Password = "updatedpassword", RoleId = Guid.NewGuid() };
            var existingUser = new User { Id = userUpdateDto.Id, UserName = "olduser", Role = new Role { Id = Guid.NewGuid(), Name = "User" } };

            var mockUserRepository = new Mock<IRepository<User>>();
            var mockRoleRepository = new Mock<IRepository<Role>>();

            _mockUnitOfWork.Setup(uow => uow.GetRepository<User>()).Returns(mockUserRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.GetRepository<Role>()).Returns(mockRoleRepository.Object);

            mockUserRepository.Setup(repo => repo.GetByIdAsync(userUpdateDto.Id, cancellationToken)).ReturnsAsync(existingUser);
            mockRoleRepository.Setup(repo => repo.GetByIdAsync(userUpdateDto.RoleId, cancellationToken)).ReturnsAsync((Role)null);

            Func<Task> action = async () => await _userService.UpdateUserAsync(userUpdateDto, cancellationToken);

            await action.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task AuthenticateAsync_ValidCredentials_ReturnsAuthenticationResponseDto()
        {
            var userName = "testuser";
            var password = "testpassword";
            var hashedPassword = "hashedtestpassword";
            var loginDto = new UserLoginDto { UserName = userName, Password = password };
            var existingUser = new User { UserName = userName, HashedPassword = hashedPassword, Role = new Role { Name = "User" } };

            var keyBytes = new byte[64];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(keyBytes);
            }
            var base64Key = Convert.ToBase64String(keyBytes);

            var mockUserRepository = new Mock<IRepository<User>>();
            _mockUnitOfWork.Setup(uow => uow.GetRepository<User>()).Returns(mockUserRepository.Object);

            mockUserRepository.Setup(repo => repo.GetByPredicateAsync(It.IsAny<Expression<Func<User, bool>>>(), cancellationToken))
                .ReturnsAsync((Expression<Func<User, bool>> predicate, CancellationToken ct) => new List<User> { existingUser });

            _mockPasswordHasher.Setup(ph => ph.VerifyHashedPassword(hashedPassword, password)).Returns(true);

            _mockConfiguration.Setup(c => c["JwtSettings:Key"]).Returns(base64Key);
            _mockConfiguration.Setup(c => c["JwtSettings:ExpiresInMinutes"]).Returns("60");
            _mockConfiguration.Setup(c => c["JwtSettings:Issuer"]).Returns("issuer");
            _mockConfiguration.Setup(c => c["JwtSettings:Audience"]).Returns("audience");

            var result = await _userService.AuthenticateAsync(loginDto, cancellationToken);

            result.Should().NotBeNull();
            result.AccessToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task AuthenticateAsync_InvalidCredentials_ThrowsUnauthorizedException()
        {
            var userName = "testuser";
            var password = "wrongpassword";
            var hashedPassword = "hashedtestpassword";
            var loginDto = new UserLoginDto { UserName = userName, Password = password };
            var existingUser = new User { UserName = userName, HashedPassword = hashedPassword, Role = new Role { Name = "User" } };

            var mockUserRepository = new Mock<IRepository<User>>();
            _mockUnitOfWork.Setup(uow => uow.GetRepository<User>()).Returns(mockUserRepository.Object);
            mockUserRepository.Setup(repo => repo.GetByPredicateAsync(user => user.UserName == userName, cancellationToken)).ReturnsAsync(new List<User> { existingUser });
            _mockPasswordHasher.Setup(ph => ph.VerifyHashedPassword(hashedPassword, password)).Returns(false);

            Func<Task> action = async () => await _userService.AuthenticateAsync(loginDto, cancellationToken);

            await action.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task CreateUserAsync_ValidUserCreateDto_ReturnsUserReadDto()
        {
            var userCreateDto = new UserCreateDto
            {
                UserName = "testuser",
                Password = "password123"
            };

            var userRepositoryMock = new Mock<IRepository<User>>();
            var roleRepositoryMock = new Mock<IRepository<Role>>();

            _mockUnitOfWork.Setup(uow => uow.GetRepository<User>()).Returns(userRepositoryMock.Object);
            _mockUnitOfWork.Setup(uow => uow.GetRepository<Role>()).Returns(roleRepositoryMock.Object);

            userRepositoryMock.Setup(repo => repo.GetByPredicateAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                cancellationToken))
                .ReturnsAsync(new List<User>());

            roleRepositoryMock.Setup(repo => repo.GetByPredicateAsync(
                It.IsAny<Expression<Func<Role, bool>>>(),
                cancellationToken))
                .ReturnsAsync(new List<Role> { new Role { Name = "User" } });

            _mockMapper.Setup(mapper => mapper.Map<User>(userCreateDto)).Returns(new User());
            _mockMapper.Setup(mapper => mapper.Map<UserReadDto>(It.IsAny<User>())).Returns(new UserReadDto());

            _mockPasswordHasher.Setup(hasher => hasher.HashPassword(userCreateDto.Password)).Returns("hashed_password");

            var result = await _userService.CreateUserAsync(userCreateDto, cancellationToken);

            Assert.NotNull(result);
            Assert.IsType<UserReadDto>(result);
        }

        [Fact]
        public async Task CreateUserAsync_DuplicateUserName_ThrowsDuplicateNameException()
        {
            var userCreateDto = new UserCreateDto { UserName = "existinguser", Password = "password" };
            var existingUser = new User { UserName = "existinguser" };

            var mockUserRepository = new Mock<IRepository<User>>();
            _mockUnitOfWork.Setup(uow => uow.GetRepository<User>()).Returns(mockUserRepository.Object);
            mockUserRepository.Setup(repo => repo.GetByPredicateAsync(user => user.UserName == userCreateDto.UserName, cancellationToken)).ReturnsAsync(new List<User> { existingUser });

            Func<Task> action = async () => await _userService.CreateUserAsync(userCreateDto, cancellationToken);

            await action.Should().ThrowAsync<DuplicateNameException>();
        }

        [Fact]
        public async Task DeleteUserByIdAsync_ExistingUserId_ReturnsUserReadDto()
        {
            var userId = Guid.NewGuid();
            var existingUser = new User { Id = userId, UserName = "testuser", Role = new Role { Id = Guid.NewGuid(), Name = "User" } };

            var mockUserRepository = new Mock<IRepository<User>>();
            _mockUnitOfWork.Setup(uow => uow.GetRepository<User>()).Returns(mockUserRepository.Object);
            mockUserRepository.Setup(repo => repo.GetByIdAsync(userId, cancellationToken)).ReturnsAsync(existingUser);
            mockUserRepository.Setup(repo => repo.Delete(existingUser)).Verifiable();
            _mockMapper.Setup(m => m.Map<UserReadDto>(existingUser)).Returns(new UserReadDto
            {
                UserName = existingUser.UserName,
                Role = new RoleReadDto { Name = existingUser.Role.Name }
            });

            var result = await _userService.DeleteUserByIdAsync(userId, cancellationToken);

            result.Should().NotBeNull();
            result.UserName.Should().Be("testuser");
        }

        [Fact]
        public async Task DeleteUserByIdAsync_NonExistingUserId_ThrowsNotFoundException()
        {
            var userId = Guid.NewGuid();

            var mockUserRepository = new Mock<IRepository<User>>();
            _mockUnitOfWork.Setup(uow => uow.GetRepository<User>()).Returns(mockUserRepository.Object);
            mockUserRepository.Setup(repo => repo.GetByIdAsync(userId, cancellationToken)).ReturnsAsync((User)null);

            Func<Task> action = async () => await _userService.DeleteUserByIdAsync(userId, cancellationToken);

            await action.Should().ThrowAsync<NotFoundException>();
        }
    }
}