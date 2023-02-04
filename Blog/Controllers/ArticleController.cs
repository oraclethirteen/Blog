using AutoMapper;
using Blog.DAL.Models;
using Blog.DAL.Repository;
using Blog.DAL.UoW;
using Blog.Extensions;
using Blog.Models.Article;
using Blog.Models.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Blog.Controllers
{
    /// <summary>
    /// Класс контроллера, реализующий основные CRUD-операции поста
    /// </summary>
    public class ArticleController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private IMapper _mapper;
        private IUnitOfWork _UoW;
        private Repository<Article> _articleRepository;
        private Repository<Tag> _tagRepository;

        public ArticleController(IMapper mapper, IUnitOfWork UoW, ILogger<UserController> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _UoW = UoW;
            _articleRepository = (Repository<Article>)_UoW.GetRepository<Article>();
            _tagRepository = (Repository<Tag>)_UoW.GetRepository<Tag>();
        }

        /// <summary>
        /// Запрос представления добавления статьи
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddArticle()
        {
            ArticleEditViewModel article = new ArticleEditViewModel();
            var tags = await Task.FromResult(_tagRepository.GetAll());
            article.CheckTags = tags.Select(t => new CheckTagViewModel { Id = t.Id, Title = t.Title, Checked = false }).ToList();

            return View(article);
        }

        /// <summary>
        /// Добавление статьи
        /// </summary>
        /// <param name="newArticle"></param>
        /// <returns>Возвращает представление просмотра статьи, в противном случае представление её добавления</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddArticle(ArticleEditViewModel newArticle)
        {
            if (ModelState.IsValid)
            {
                var article = _mapper.Map<Article>(newArticle);

                article.Date = DateTime.Now;
                article.UserId = User.Identity.GetUserId();

                await _articleRepository.Create(article);
                _logger.LogInformation($"Пользователь {User.Identity.Name} добавил статью {article.Title}.");
                return RedirectToAction("ViewArticle", new { id = article.Id });
            }
            else
            {
                _logger.LogInformation(ModelState.GetAllError());
            }

            return View(newArticle);
        }

        /// <summary>
        /// Получение представления редактирования статьи
        /// </summary>
        /// <param name="id">Идентификатор редактируемой статьи</param>
        /// <returns>Вовзращает представлене редактирования статьи</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditArticle(int id)
        {
            Article article = (await Task.FromResult(_articleRepository.Get(p => p.Id == id, null, "ArticleTags"))).Result.FirstOrDefault();
            List<Tag> allTags = await Task.FromResult(_tagRepository.GetAll().ToList());
            ArticleEditViewModel articleEdit = _mapper.Map<ArticleEditViewModel>(article);
            articleEdit.CheckTags = allTags.Select(t => new CheckTagViewModel
            {
                Id = t.Id,
                Title = t.Title,
                Checked = article.ArticleTags.Any(pt => pt.TagId == t.Id)
            }).ToList();

            return View(articleEdit);
        }

        /// <summary>
        /// Редактирование статьи
        /// </summary>
        /// <param name="model">Модель представления статьи</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> EditArticle(ArticleEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var article = await _articleRepository.Get(model.Id);

                await _articleRepository.Update(article);
                _logger.LogInformation($"Пользователь {User.Identity.Name} отредактировал статью {article.Title}");

                return RedirectToAction("ViewArticle", article.Id);
            }
            else
            {
                _logger.LogInformation(ModelState.GetAllError());
            }

            return View(model);
        }

        /// <summary>
        /// Удаление статьи
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            Article article = await _articleRepository.Get(id);
            if (article is null)
            {
                ViewBag.Message = $"Статья (ID = {id}) не найдена";
                return View("ArticleList");
            }

            await _articleRepository.Delete(article);
            _logger.LogInformation($"Пользователь {User.Identity.Name} удалил статью {article.Title}");

            return View("ArticleList");
        }

        /// <summary>
        /// Получение списка постов
        /// </summary>
        /// <returns>Представление списка постов</returns>
        [HttpGet]
        public async Task<IActionResult> ArticleList()
        {
            var articleList = await Task.FromResult(_articleRepository.GetAll());
            List<ArticleViewModel> resultArticleList = _mapper.Map<List<ArticleViewModel>>(articleList);

            return View(resultArticleList);
        }

        /// <summary>
        /// Получить статью по ID
        /// </summary>
        /// <param name="id">Идентификатор статьи</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ViewArticle(int id)
        {
            Article article = (await Task.FromResult(_articleRepository.Get(p => p.Id == id,
                null,
                "ArticleList",
                "User",
                "Comments"))).Result.FirstOrDefault();

            if (article == null)
            {
                _logger.LogInformation($"Статья (ID = {id}) не найдена");
                return View("NotFound");
            }

            ArticleViewModel articleView = _mapper.Map<ArticleViewModel>(article);
            return View(articleView);
        }
    }
}
