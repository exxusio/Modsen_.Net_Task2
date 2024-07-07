namespace BusinessLogicLayer.Dtos.OrderItems
{
    public class OrderItemCreateNewOrderDto
    {
        public int Amount { get; set; }
        public Guid ProductId { get; set; }
    }
}
