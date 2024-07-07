using System.Text.Json.Serialization;

namespace BusinessLogicLayer.Dtos.OrderItems
{
    public class OrderItemCreateDto
    {
        public int Amount { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        [JsonIgnore]
        public string UserName { get; set; }
    }
}
