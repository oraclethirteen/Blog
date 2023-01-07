using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class ArticleEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле 'Название' обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Название", Prompt = "Введите название")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Поле 'Текст' обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Текст", Prompt = "Введите текст")]
        public string Content { get; set; }

    }
}
