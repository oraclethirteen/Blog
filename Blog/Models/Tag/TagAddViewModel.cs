using System.ComponentModel.DataAnnotations;

namespace Blog.Models.Tag
{
    public class TagAddViewModel
    {
        [Required(ErrorMessage = "* поле 'Название' обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Название", Prompt = "Введите название")]
        public string Title { get; set; }
    }
}
