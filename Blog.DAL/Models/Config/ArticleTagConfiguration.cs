using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Models.Config
{
    /// <summary>
    /// Кофигурация модели связи статьи и тега
    /// </summary>
    public class ArticleTagConfiguration : IEntityTypeConfiguration<ArticleTag>
    {
        public void Configure(EntityTypeBuilder<ArticleTag> builder)
        {
            builder.ToTable("ArticleTag").HasKey(x => new { x.ArticleId, x.TagId });

            builder.HasOne(x => x.Article)
                   .WithMany(t => t.ArticleTags)
                   .HasForeignKey(x => x.ArticleId);

            builder.HasOne(x => x.Tag)
                   .WithMany(t => t.ArticleTags)
                   .HasForeignKey(x => x.TagId);
            ;
        }
    }
}
