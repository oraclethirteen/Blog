using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.DAL.Models.Config
{
    // Кофигурация модели связи статьи и тега
    public class ArticleTagConfiguration : IEntityTypeConfiguration<ArticleTag>
    {
        public void Configure(EntityTypeBuilder<ArticleTag> builder)
        {
            builder.ToTable("ArticleTag").HasKey(pt => new { pt.ArticleId, pt.TagId });

            builder.HasOne(pt => pt.Article)
                   .WithMany(t => t.ArticleTags)
                   .HasForeignKey(pt => pt.ArticleId);

            builder.HasOne(pt => pt.Tag)
                   .WithMany(t => t.ArticleTags)
                   .HasForeignKey(pt => pt.TagId);
            ;
        }
    }
}
