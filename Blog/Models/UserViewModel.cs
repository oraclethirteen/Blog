﻿using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Имя", Prompt = "Введите имя")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия", Prompt = "Введите фамилию")]
        public string LastName { get; set; }

        [Display(Name = "Email", Prompt = "example@example.com")]
        public string Email { get; set; }

        [Display(Name = "Логин", Prompt = "Введите логин")]
        public string Login { get; set; }

        public string FullName { get { return string.Concat(FirstName, " ", LastName); } }

    }
}
