using AutoMapper;
using Blog.DAL.Models;
using Blog.DAL.Repository;
using Blog.DAL.UoW;
using Blog.Models.Article;
using Blog.Models.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    /// <summary>
    /// Класс контроллера, реализующий основные CRUD-операции поста
    /// </summary>
    public class ArticleController : Controller
    {
        private IMapper _mapper;
        private IUnitOfWork _UoW;
        private Repository<Article> _articleRepository;
        private Repository<Tag> _tagRepository;

        public ArticleController(IMapper mapper, IUnitOfWork UoW)
        {
            _mapper = mapper;
            _UoW = UoW;
            _articleRepository = (Repository<Article>)_UoW.GetRepository<Article>();
            _tagRepository = (Repository<Tag>)_UoW.GetRepository<Tag>();
        }

        [HttpGet]
        public async Task<IActionResult> AddArticle()
        {

            ArticleEditViewModel article = new ArticleEditViewModel();

            var tags = await Task.FromResult(_tagRepository.GetAll());

            article.CheckTags = tags.Select(t => new CheckTagViewModel { Id = t.Id, Title = t.Title, Checked = false }).ToList();

            return View(article);
        }

        [HttpPost]
        public async Task<string> AddArticle(ArticleEditViewModel newArticle)
        {
            if (ModelState.IsValid)
            {
                var article = _mapper.Map<Article>(newArticle);

                article.Date = DateTime.Now;

                await _articleRepository.Create(article);
                return "Действие выполнено успешно";
            }
            return string.Join("\r\n", ModelState.Values.SelectMany(v => v.Errors));
        }

        [HttpGet]
        public async Task<IActionResult> ViewArticle(int id)
        {
            Article article = (await Task.FromResult(_articleRepository.Get(p => p.Id == id,
                null,
                "ArticleTags",
                "User",
                "Comments"))).Result.FirstOrDefault();

            List<Tag> allTags = await Task.FromResult(_tagRepository.GetAll().ToList());
            ArticleViewModel articleView = _mapper.Map<ArticleViewModel>(article);

            return View(articleView);
        }

        [HttpGet]
        public async Task<IActionResult> ArticleList()
        {
            var articleList = await Task.FromResult(_articleRepository.GetAll());
            List<ArticleViewModel> resultArticleList = _mapper.Map<List<ArticleViewModel>>(articleList);

            return View(resultArticleList);
        }

        [HttpGet]
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

        //[Authorize]
        [HttpPut]
        public async Task<string> Update(ArticleEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var article = await _articleRepository.Get(model.Id);

                await _articleRepository.Update(article);

                return "Действие выполнено успешно";
            }
            else
            {
                return string.Join("\r\n", ModelState.Values.SelectMany(v => v.Errors));
            }
        }


        [Authorize]
        [Route("DeleteArticle")]
        [HttpDelete]
        public async Task<string> Delete(int articleId)
        {
            Article article = await _articleRepository.Get(articleId);
            if (article is null)
            {
                return $"Пост (ID = {articleId}) не найден";
            }

            await _articleRepository.Delete(article);
            return "Действие выполнено успешно";
        }  
    }
}
