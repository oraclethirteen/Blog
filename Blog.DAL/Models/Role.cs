namespace Blog.DAL.Models
{
    public class Role
    {
        public ICollection<UserRole> UserRoles { get; set; }

        public int Id { get; set; }
        public string Title { get; set; }
    }
}
