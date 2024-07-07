using BusinessLogicLayer.Dtos.OrderItems;
using System.Text.Json.Serialization;

namespace BusinessLogicLayer.Dtos.Orders
{
    public class OrderCreateDto
    {
        [JsonIgnore]
        public string? UserName { get; set; }
        public ICollection<OrderItemCreateNewOrderDto> OrderItems { get; set; }
    }
}
