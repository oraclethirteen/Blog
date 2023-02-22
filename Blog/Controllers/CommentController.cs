using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Blog.Models;
using Blog.Extensions;
using Blog.BLL.Services;
using Blog.BLL.Models;
using Blog.BLL.Response;

namespace Blog.Controllers
{
    // Класс контроллера, реализующий основные CRUD-операции комментария
    public class CommentController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private IMapper _mapper;
        private ICommentService _commentService;

        public CommentController(IMapper mapper, ICommentService commentService, ILogger<UserController> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _commentService = commentService;
            _logger = logger;
        }

        /// <summary>
        /// Запрос представления добавления комментария
        /// </summary>
        /// <param name="articleId"> Идентификатор комментируемой статьи </param>
        /// <returns> View добавления комментария </returns>
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
        /// <param name="newComment"> ViewModel комментария</param>
        /// <returns> View комментария </returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentEditViewModel newComment)
        {
            if (ModelState.IsValid)
            {
                var comment = _mapper.Map<CommentDomain>(newComment);
                comment.UserId = User.Identity.GeUsertId();
                EntityBaseResponse<CommentDomain> result = await _commentService.Add(comment);
                if (result.Success)
                {
                    _logger.LogInformation($"Пользователь {User.Identity.Name} добавил комментарий.");
                    return RedirectToAction("ViewArticle", "Article", new { id = comment.ArticleId });
                }
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
        /// <param name="model"> ViewModel редактирования </param>
        /// <returns> View комментария </returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditComment(CommentEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var comment = _mapper.Map<CommentDomain>(model);

                await _commentService.Update(comment);

                return RedirectToAction("ArticleView", "Article", new { id = model.ArticleId });
            }
            else
            {
                _logger.LogInformation(ModelState.GetAllError());
            }

            return View(model);
        }

        /// <summary>
        /// Удаление комментария
        /// </summary>
        /// <param name="id"> Идентификатор комментария </param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            EntityBaseResponse<CommentDomain> comment = await _commentService.Get(id);

            if (!comment.Success)
            {
                return View("NotFound");
            }

            comment = await _commentService.Delete(comment.Entity);
            if (comment.Success)
            {
                _logger.LogInformation($"Пользователь {User.Identity.Name} удалил комментарий.");

            }

            return RedirectToAction("CommentList");
        }

        /// <summary>
        /// Список всех комментариев
        /// </summary>
        /// <returns> View списка комментариев </returns>
        [HttpGet]
        public async Task<IActionResult> CommentList()
        {
            var commentList = await Task.FromResult(_commentService.GetAll());
            List<CommentViewModel> resultCommentList = _mapper.Map<List<CommentViewModel>>(commentList.Entity);

            return View(resultCommentList);
        }

        /// <summary>
        /// Список комментариев к статье
        /// </summary>
        /// <returns> View списка комментариев к статье </returns>
        [HttpGet]
        public async Task<IActionResult> ArticleCommentList(int articleId)
        {
            var commentList = await Task.FromResult(_commentService.GetByArticle(articleId));
            List<CommentViewModel> resultCommentList = _mapper.Map<List<CommentViewModel>>(commentList);

            return View(resultCommentList);
        }

        /// <summary>
        /// Получение комментария по ID
        /// </summary>
        /// <param name="id"> Идентификатор комментария </param>
        /// <returns> View комментария </returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ViewComment(int id)
        {
            var commentResponse = await _commentService.Get(id);

            if (!commentResponse.Success)
            {
                _logger.LogInformation(commentResponse.Message);
                return View("NotFound");
            }

            return View(_mapper.Map<TagViewModel>(commentResponse.Entity));
        }
    }
}
