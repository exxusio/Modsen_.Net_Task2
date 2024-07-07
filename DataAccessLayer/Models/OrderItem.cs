namespace DataAccessLayer.Models
{
    public class OrderItem : BaseModel
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Amount { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
