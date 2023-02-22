using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class CommentEditViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Display(Name = "ID статьи")]
        [HiddenInput]
        [Range(1, int.MaxValue, ErrorMessage = "Не указан ID статьи")]
        public int ArticleId { get; set; }

        [Required(ErrorMessage = "* поле 'Комментарий' обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Коментарии", Prompt = "Введите комментарий")]
        public string Content { get; set; }
    }
}
