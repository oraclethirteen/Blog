using System.ComponentModel.DataAnnotations;

namespace Blog.Models.Role
{
    public class RoleEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "* поле 'Название' обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Наименование", Prompt = "Введите название")]
        public string Title { get; set; }
    }
}
