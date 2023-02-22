using System.ComponentModel.DataAnnotations;

namespace Blog.BLL.Models
{
    // Доменный класс тега
    public class TagDomain
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "* поле 'Название' обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Название", Prompt = "Введите название")]
        public string Title { get; set; }

        public ICollection<ArticleDomain> Articles { get; set; }
    }
}