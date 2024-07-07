using DataAccessLayer.Data.Interfaces;
using DataAccessLayer.Models;

namespace DataAccessLayer.Data.Implementations
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }
    }
}
