using System.ComponentModel.DataAnnotations;

namespace Blog.BLL.Models
{
    // Доменный класс статьи
    public class ArticleDomain
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "* поле 'Заголовок' обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Заголовок", Prompt = "Введите заголовок")]
        public string Title { get; set; }

        [Required(ErrorMessage = "* поле 'Текст' обязательно для заполнения")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Текст", Prompt = "Введите текст")]
        public string Content { get; set; }
        public int Views { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }

        public UserDomain? User { get; set; }

        public ICollection<TagDomain> Tags { get; set; }
        public ICollection<CommentDomain> Comments { get; set; }
    }
}