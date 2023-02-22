using System.ComponentModel.DataAnnotations;

namespace Blog.BLL.Models
{
    // Доменный класс роли
    public class RoleDomain
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "* поле 'Название' обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Название", Prompt = "Введите название")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание", Prompt = "Введите описание")]
        public string Description { get; set; }

        public ICollection<UserDomain> Users { get; set; }
    }
}