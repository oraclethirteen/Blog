using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Models.Config
{
    /// <summary>
    /// Кофигурация модели связи пользователя и роли
    /// </summary>
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRole").HasKey(x => new { x.UserId, x.RoleId });

            builder.HasOne(x => x.User)
                   .WithMany(t => t.UserRoles)
                   .HasForeignKey(c => c.UserId);

            builder.HasOne(x => x.Role)
                   .WithMany(t => t.UserRoles)
                   .HasForeignKey(c => c.RoleId);
        }
    }
}
