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
                .ThenInclude(r => r.Role)
                .FirstOrDefault(l => l.Login == login);
        }
    }
}
