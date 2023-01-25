using Blog.Models.Tag;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models.Article
{
    public class ArticleEditViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "* поле 'Название' обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Заголовок", Prompt = "Введите название")]
        public string Title { get; set; }

        [Required(ErrorMessage = "* поле 'Текст' обязательно для заполнения")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Текст", Prompt = "Введите текст")]
        public string Content { get; set; }

        public List<CheckTagViewModel> CheckTags { get; set; }
    }
}
