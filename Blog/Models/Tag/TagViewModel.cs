using Blog.DAL.Models;
using System.Collections.Generic;

namespace Blog.Models.Tag
{
    public class TagViewModel
    {
        public ICollection<Blog.DAL.Models.Article> Articles { get; set; }

        public int Id { get; set; }
        public string Title { get; set; }

        public int CountArticle => Articles?.Count ?? 0;
    }
}
