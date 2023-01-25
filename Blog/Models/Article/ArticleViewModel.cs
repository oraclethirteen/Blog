using Blog.Models.Comment;
using Blog.Models.Tag;

namespace Blog.Models.Article
{
    public class ArticleViewModel
    {
        public List<TagCustomViewModel> Tags { get; set; }
        public List<CommentViewModel> Comments { get; set; }

        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int Views { get; set; }
    }
}
