using BusinessLogicLayer.Dtos.Products;

namespace BusinessLogicLayer.Dtos.OrderItems
{
    public class OrderItemReadDto
    {
        public int Amount { get; set; }
        public ProductReadDto Product { get; set; }
    }
}
