using BusinessLogicLayer.Dtos.Users;

namespace Tests.Profiles
{
    public class UserMappingProfileTests
    {
        private IMapper _mapper;

        public UserMappingProfileTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserMappingProfile());
                cfg.AddProfile(new RoleMappingProfile());
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void Mapping_Configuration_IsValid()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void UserCreateDto_To_User_Mapping()
        {
            var userCreateDto = new UserCreateDto
            {
                UserName = "TestUser",
                Password = "TestPassword"
            };

            var user = _mapper.Map<User>(userCreateDto);

            Assert.Equal(userCreateDto.UserName, user.UserName);
        }

        [Fact]
        public void UserUpdateDto_To_User_Mapping()
        {
            var userUpdateDto = new UserUpdateDto
            {
                Id = Guid.NewGuid(),
                UserName = "UpdatedUser",
                Password = "UpdatedPassword",
                RoleId = Guid.NewGuid()
            };

            var user = _mapper.Map<User>(userUpdateDto);

            Assert.Equal(userUpdateDto.Id, user.Id);
            Assert.Equal(userUpdateDto.UserName, user.UserName);
            Assert.Equal(userUpdateDto.RoleId, user.RoleId);
        }

        [Fact]
        public void User_To_UserReadDto_Mapping()
        {
            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = "UserRole"
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "TestUser",
                Role = role
            };

            var userReadDto = _mapper.Map<UserReadDto>(user);

            Assert.Equal(user.UserName, userReadDto.UserName);
            Assert.Equal(user.Role.Name, userReadDto.Role.Name);
        }
    }
}
