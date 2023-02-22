using System.ComponentModel.DataAnnotations;

namespace Blog.BLL.Models
{
    // Доменный класс комментария
    public class CommentDomain
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "* поле 'Коментарии' обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Коментарии", Prompt = "Введите комментарий")]
        public string Content { get; set; }

        public DateTime Date { get; set; }

        [Display(Name = "ID статьи")]
        [Range(1, int.MaxValue, ErrorMessage = "Не указан ID статьи")]
        public int ArticleId { get; set; }

        public int UserId { get; set; }
        public ArticleDomain? Article { get; set; }
        public UserDomain? User { get; set; }
    }
}