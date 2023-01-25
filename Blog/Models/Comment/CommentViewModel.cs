using Blog.Models.Article;

namespace Blog.Models.Comment
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public ArticleCustomViewModel Article { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}
