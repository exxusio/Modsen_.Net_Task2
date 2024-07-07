using System.Text.Json.Serialization;

namespace BusinessLogicLayer.Dtos.Products
{
    public class ProductUpdateDto
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public Guid CategoryId { get; set; }
    }
}
