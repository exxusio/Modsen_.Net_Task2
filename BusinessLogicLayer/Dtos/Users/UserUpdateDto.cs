using System.Text.Json.Serialization;

namespace BusinessLogicLayer.Dtos.Users
{
    public class UserUpdateDto
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Guid RoleId { get; set; }
    }
}
