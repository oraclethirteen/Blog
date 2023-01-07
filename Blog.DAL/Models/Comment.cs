namespace Blog.DAL.Models
{
    public class Comment
    {
        public User User { get; set; }
        public Article Article { get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ArticleId { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}
