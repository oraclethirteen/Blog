using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class UserEditViewModel
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

        [Required(ErrorMessage = "* поле 'Логин' обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Логин", Prompt = "Введите логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "* поле 'Email' обязательно для заполнения")]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "example@domain.com")]
        public string Email { get; set; }

        [Required(ErrorMessage = "* поле 'Пароль' обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Prompt = "Введите пароль")]
        [StringLength(100, ErrorMessage = "* поле {0} должно иметь минимум {2} и максимум {1} символов", MinimumLength = 5)]
        public string Password { get; set; }

        [Required(ErrorMessage = "* необходимо подтвердить пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль", Prompt = "Введите пароль повторно")]
        public string PasswordConfirm { get; set; }

        public string FullName { get { return string.Concat(FirstName, " ", LastName); } }

        public List<CheckRoleViewModel> CheckRoles { get; set; }
    }
}
