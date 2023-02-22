namespace Blog.Models
{
    public class TagViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<ArticleCustomViewModel> Articles { get; set; }

        public int CountArticle => Articles?.Count ?? 0;
    }
}
