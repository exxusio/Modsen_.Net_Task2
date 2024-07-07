namespace DataAccessLayer.Models
{
    public class Role : BaseModel
    {
        public string Name { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
