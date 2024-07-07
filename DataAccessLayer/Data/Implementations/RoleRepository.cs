using DataAccessLayer.Data.Interfaces;
using DataAccessLayer.Models;

namespace DataAccessLayer.Data.Implementations
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }
    }
}
