using System.Text.Json.Serialization;

namespace BusinessLogicLayer.Dtos.Categories
{
    public class CategoryUpdateDto
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
