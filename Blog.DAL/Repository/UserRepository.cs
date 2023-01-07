using Blog.DAL.Models;

namespace Blog.DAL.Repository
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(BlogDbContext db) : base(db)
        {

        }

        // Метод, возвращающий пользователя по логину
        public User GetByLogin(string login)
        {
            return Set.FirstOrDefault(u => u.Login == login);
        }
    }
}
