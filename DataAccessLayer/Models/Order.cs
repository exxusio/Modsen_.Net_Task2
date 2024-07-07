namespace DataAccessLayer.Models
{
    public class Order : BaseModel
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
