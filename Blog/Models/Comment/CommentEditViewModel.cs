using System.ComponentModel.DataAnnotations;

namespace Blog.Models.Comment
{
    public class CommentEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "* поле 'Комментарий' обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Комментарий", Prompt = "Введите комментарий")]
        public string Content { get; set; }

    }
}
