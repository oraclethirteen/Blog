using Blog.DAL.Models;

namespace Blog.BLL.Models
{
    /// <summary>
    /// Доменный класс тега
    /// </summary>
    public class TagDomain
    {
        public ICollection<Article> Articles { get; set; }

        public int Id { get; set; }
        public string Title { get; set; }

        public static TagDomain CreateTagDomain(Tag tag)
        {
            return Helper.Mapper.Map<TagDomain>(tag);
        }   
    }
}
