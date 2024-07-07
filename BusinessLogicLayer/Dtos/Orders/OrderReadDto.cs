using BusinessLogicLayer.Dtos.OrderItems;

namespace BusinessLogicLayer.Dtos.Orders
{
    public class OrderReadDto
    {
        public Guid Id { get; set; }
        public ICollection<OrderItemReadDto> OrderItems { get; set; }
    }
}
