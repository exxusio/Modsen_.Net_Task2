using DataAccessLayer.Data.Interfaces;
using DataAccessLayer.Models;

namespace DataAccessLayer.Data.Implementations
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }
    }
}
