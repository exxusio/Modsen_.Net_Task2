namespace DataAccessLayer.Models
{
    public class User : BaseModel
    {
        public Guid RoleId { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
