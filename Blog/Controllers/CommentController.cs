using AutoMapper;
using Blog.DAL.Models;
using Blog.DAL.Repository;
using Blog.DAL.UoW;
using Blog.Extensions;
using Blog.Models.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    /// <summary>
    /// Класс контроллера, реализующий основные CRUD-операции комментария
    /// </summary>
    public class CommentController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private IMapper _mapper;
        private IUnitOfWork _UoW;
        private Repository<Comment> _commentRepository;

        public CommentController(IMapper mapper, IUnitOfWork UoW, ILogger<UserController> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _UoW = UoW;
            _commentRepository = (Repository<Comment>)_UoW.GetRepository<Comment>();
            _logger = logger;
        }

        /// <summary>
        /// Запрос представления добавления комментария
        /// </summary>
        /// <param name="articleId">Идентификатор комментируемой статьи</param>
        /// <returns>View добавления комментария</returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddComment(int articleId)
        {
            CommentEditViewModel newComment = new CommentEditViewModel();
            newComment.ArticleId = articleId;
            return PartialView(newComment);
        }

        /// <summary>
        /// Добавления комментария
        /// </summary>
        /// <param name="newComment">Представление комментария</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentEditViewModel newComment)
        {
            if (ModelState.IsValid)
            {
                var comment = _mapper.Map<Comment>(newComment);

                comment.Date = DateTime.Now;
                comment.UserId = User.Identity.GetUserId();

                await _commentRepository.Create(comment);
                _logger.LogInformation($"Пользователь {User.Identity.Name} добавил комментарий");
                return View();
            }
            else
            {
                _logger.LogInformation(ModelState.GetAllError());
            }

            return View(newComment);
        }

        /// <summary>
        /// Редактирования комментария
        /// </summary>
        /// <param name="model">Модель представления редактирования комментария</param>
        /// <returns>строка результа</returns>
        [Authorize]
        [HttpPut]
        public async Task<string> EditComment(CommentEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var comment = await _commentRepository.Get(model.Id);

                await _commentRepository.Update(comment);

                return "Действие выполнено успешно";
            }
            else
            {
                return string.Join("\r\n", ModelState.Values.SelectMany(v => v.Errors));
            }
        }

        /// <summary>
        /// Удаление комментария
        /// </summary>
        /// <param name="commentId">Идентификатор комментария</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        public async Task<string> Delete(int commentId)
        {
            Comment comment = await _commentRepository.Get(commentId);
            if (comment is null)
            {
                return $"Статья (ID = {commentId}) не найдена";
            }

            await _commentRepository.Delete(comment);
            _logger.LogInformation($"Пользователь {User.Identity.Name} удалил комментарий");
            return "Пост удален.";


        }

        /// <summary>
        /// Список всех комментариев
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CommentList()
        {
            var commentList = await Task.FromResult(_commentRepository.GetAll());
            List<CommentViewModel> resultCommetList = _mapper.Map<List<CommentViewModel>>(commentList);

            return View(resultCommetList);
        }

        /// <summary>
        /// Список комментариев к статье
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ArticleCommentList(int articleId)
        {
            var commentList = await Task.FromResult(_commentRepository.Get(c => c.ArticleId == articleId, c => c.OrderBy(c => c.Date)));
            List<CommentViewModel> resultCommetList = _mapper.Map<List<CommentViewModel>>(commentList);

            return View(resultCommetList);
        }

        /// <summary>
        /// Получение комментария по ID
        /// </summary>
        /// <param name="commentId"Идентификатор комментария></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<CommentViewModel> ViewComment(int commentId)
        {
            CommentViewModel resultComment = new CommentViewModel();

            Comment comment = await _commentRepository.Get(commentId);

            return _mapper.Map<CommentViewModel>(comment);
        }
    }
}
