using Blog.DAL.Models;

namespace Blog.BLL.Models
{
    public class UserDomain
    {
        public ICollection<Role> Roles { get; set; }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public UserDomain() { }

        public static UserDomain CreateUserDomain(User user)
        {
            return Helper.Mapper.Map<UserDomain>(user);
        } 
    }
}
