using System.ComponentModel.DataAnnotations;

namespace Blog.BLL.Models
{
    // Доменный класс пользователя
    public class UserDomain
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "* поле 'Имя' обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Имя", Prompt = "Введите имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "* поле 'Фамилия' обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Фамилия", Prompt = "Введите фамилию")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "* поле 'Email' обязательно для заполнения")]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "example@domain.com")]
        public string Email { get; set; }

        [Required(ErrorMessage = "* поле 'Логин' обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Логин", Prompt = "Введите логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "* поле 'Пароль' обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Prompt = "Введите пароль")]
        [StringLength(100, ErrorMessage = "* поле {0} должно иметь от {2} до {1} символов", MinimumLength = 5)]
        public string Password { get; set; }

        public ICollection<RoleDomain> Roles { get; set; }

        public UserDomain()
        {

        }
    }
}