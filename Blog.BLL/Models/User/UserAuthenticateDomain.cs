using System.ComponentModel.DataAnnotations;

namespace Blog.BLL.Models
{
    // Доменный класс аутентификации пользователя
    public class UserAuthenticateDomain
    {
        [Required(ErrorMessage = "* поле 'Логин' обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Логин", Prompt = "Введите логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "* поле 'Пароль' обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Prompt = "Введите пароль")]
        [StringLength(100, ErrorMessage = "* поле {0} должно иметь от {2} до {1} символов", MinimumLength = 5)]
        public string Password { get; set; }
    }
}