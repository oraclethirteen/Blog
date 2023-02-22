using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.DAL.Models.Config
{
    // Кофигурация модели связи пользователя и роли
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRole").HasKey(ur => new { ur.UserId, ur.RoleId });

            builder.HasOne(ur => ur.User)
                   .WithMany(u => u.UserRoles)
                   .HasForeignKey(ur => ur.UserId);

            builder.HasOne(ur => ur.Role)
                   .WithMany(r => r.UserRoles)
                   .HasForeignKey(ur => ur.RoleId);
        }
    }
}
