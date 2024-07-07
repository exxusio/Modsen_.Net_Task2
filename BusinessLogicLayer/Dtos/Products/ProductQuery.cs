namespace BusinessLogicLayer.Dtos.Products
{
    public class ProductQuery
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float MinPrice { get; set; }
        public float MaxPrice { get; set; }
        public Guid CategoryId { get; set; }
    }
}
