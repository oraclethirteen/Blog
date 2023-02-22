namespace Blog.Models
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime Date { get; set; }

        public ArticleCustomViewModel Article { get; set; }
    }
}
