using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class TagEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "* поле 'Название' обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Название", Prompt = "Введите название")]
        public string Title { get; set; }
    }
}
