using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class TagEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле 'Название тега' обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Название тега", Prompt = "Введите название тега")]
        public string Title { get; set; }

    }
}
