using System.Collections.ObjectModel;

namespace Blog.DAL.Models
{
    public class Article
    {
        public ObservableCollection<ArticleTag> ArticleTags { get; set; }
        public ObservableCollection<Comment> Comments { get; set; }

        public Article()
        {
            ArticleTags = new ObservableCollection<ArticleTag>();
            Comments = new ObservableCollection<Comment>();
        }

        public User User { get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int Views { get; set; }
    }
}
