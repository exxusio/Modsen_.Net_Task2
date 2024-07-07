using BusinessLogicLayer.Dtos.Roles;

namespace Tests.Profiles
{
    public class RoleMappingProfileTests
    {
        private IMapper _mapper;

        public RoleMappingProfileTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
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
        public void Role_To_RoleReadDto_Mapping()
        {
            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = "User"
            };

            var roleReadDto = _mapper.Map<RoleReadDto>(role);

            Assert.Equal(role.Name, roleReadDto.Name);
        }
    }
}