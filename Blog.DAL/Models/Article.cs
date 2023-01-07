namespace Blog.DAL.Models
{
    public class Article
    {
        public ICollection<ArticleTag> ArticleTags { get; set; }

        public User User { get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}
