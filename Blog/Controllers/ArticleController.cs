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
    // Класс контроллера, реализующий основные CRUD-операции статьи
    public class ArticleController : Controller
    {
        private readonly ILogger<ArticleController> _logger;
        private IMapper _mapper;
        private IArticleService _articleService;

        public ArticleController(IMapper mapper, IArticleService articleService, ILogger<ArticleController> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _articleService = articleService;
        }

        /// <summary>
        /// Запрос представления добавления статьи
        /// </summary>
        /// <returns> View добавления статьи </returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddArticle()
        {
            ArticleEditViewModel article = new ArticleEditViewModel();
            var tags = await Task.FromResult(_articleService.GetAllTags());
            article.CheckTags = tags.Select(t => new CheckTagViewModel { Id = t.Id, Title = t.Title, Checked = false }).ToList();

            return View(article);
        }

        /// <summary>
        /// Добавление статьи
        /// </summary>
        /// <param name="newArticle"> ViewModel статьи </param>
        /// <returns> Возвращает представление просмотра статьи, в противном случае представление её добавления </returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddArticle(ArticleEditViewModel newArticle)
        {
            if (ModelState.IsValid)
            {
                var article = _mapper.Map<ArticleDomain>(newArticle);

                article.UserId = User.Identity.GeUsertId();

                EntityBaseResponse<ArticleDomain> result = await _articleService.Add(article);

                if (result.Success)
                {
                    _logger.LogInformation($"Пользователь {User.Identity.Name} добавил статью {article.Title}.");
                    return RedirectToAction("ViewArticle", new { result.Entity.Id });
                }
                else
                {
                    View(newArticle);
                }
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
        /// <param name="id"> Идентификатор редактируемой статьи </param>
        /// <returns> Вовзращает представлене редактирования статьи </returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditArticle(int id)
        {
            EntityBaseResponse<ArticleDomain> article = await _articleService.Get(id);
            if (article.Success)
            {
                List<TagDomain> allTags = _articleService.GetAllTags().ToList();
                ArticleEditViewModel articleEdit = _mapper.Map<ArticleEditViewModel>(article.Entity);

                articleEdit.CheckTags = allTags.Select(t => new CheckTagViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Checked = article.Entity.Tags.Any(pt => pt.Id == t.Id)
                }).ToList();

                return View(articleEdit);
            }
            else
            {
                return View("NotFound");
            }
        }

        /// <summary>
        /// Редактирование статьи
        /// </summary>
        /// <param name="model"> ViewModel редактирования статьи </param>
        /// <returns> View редактирования статьи </returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditArticle(int id, ArticleEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var article = _mapper.Map<ArticleDomain>(model);
                await _articleService.Update(article);
                _logger.LogInformation($"Пользователь {User.Identity.Name} отредактировал статью {article.Title}");

                return RedirectToAction("ViewArticle", new { id });
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
        /// <param name="id"> Идентификатор статьи </param>
        /// <returns> View списка статей </returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            EntityBaseResponse<ArticleDomain> article = await _articleService.Get(id);
            if (!article.Success)
            {
                return View("NotFound");
            }

            article = await _articleService.Delete(article.Entity);
            if (!article.Success)
            {
                _logger.LogInformation($"Пользователь {User.Identity.Name} удалил статью {article.Entity.Title}.");
            }

            return RedirectToAction("ArticleList");
        }

        /// <summary>
        /// Получение списка статей
        /// </summary>
        /// <returns> View списка статей </returns>
        [HttpGet]
        public async Task<IActionResult> ArticleList()
        {
            var articleList = await Task.FromResult(_articleService.GetAll());
            List<ArticleViewModel> resultArticleList = _mapper.Map<List<ArticleViewModel>>(articleList.Entity);

            return View(resultArticleList);
        }

        /// <summary>
        /// Получить статью по ID
        /// </summary>
        /// <param name="id"> Идентификатор статьи </param>
        /// <returns> View статьи </returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ViewArticle(int id)
        {
            var articleResponse = await _articleService.Get(id);

            if (!articleResponse.Success)
            {
                _logger.LogInformation(articleResponse.Message);

                return View("NotFound");
            }

            return View(_mapper.Map<ArticleViewModel>(articleResponse.Entity));
        }
    }
}
