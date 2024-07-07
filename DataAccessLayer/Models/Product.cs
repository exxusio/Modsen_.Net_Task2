namespace DataAccessLayer.Models
{
    public class Product : BaseModel
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public virtual Category Category { get; set; }
    }
}
