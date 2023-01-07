namespace Blog.DAL.Models
{
    public class ArticleTag
    {
        public Article Article { get; set; }
        public Tag Tag { get; set; }
        public int ArticleId { get; set; }
        public int TagId { get; set; }
    }
}
