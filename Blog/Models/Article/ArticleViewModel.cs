namespace Blog.Models
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public string Author { get; set; }
        public int Views { get; set; }
        public DateTime Date { get; set; }

        public List<TagCustomViewModel> Tags { get; set; }
        public List<CommentViewModel> Comments { get; set; }
    }
}
