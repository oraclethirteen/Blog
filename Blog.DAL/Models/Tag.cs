namespace Blog.DAL.Models
{
    public class Tag
    {
        public ICollection<ArticleTag> ArticleTags { get; set; }

        public int Id { get; set; }
        public string Title { get; set; }
    }
}
