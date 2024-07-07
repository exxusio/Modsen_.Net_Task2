using DataAccessLayer.Data.Interfaces;
using DataAccessLayer.Models;

namespace DataAccessLayer.Data.Implementations
{
    public class OrderItemRepository : BaseRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}
