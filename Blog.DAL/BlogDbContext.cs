using Blog.DAL.Models.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.DAL
{
    /// <summary>
    /// Класс контекста, необходимый для доступа к сущностям БД
    /// </summary>
    public class BlogDbContext : DbContext
    {
        ServiceCollection _serviceProvider = new ServiceCollection();

        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new ArticleTagConfiguration());
        }
    }
}
