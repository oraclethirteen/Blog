using Blog.DAL.Models;
using Microsoft.EntityFrameworkCore;

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
            return _db.Set<User>()
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefault(u => u.Login == login);
        }
    }
}
