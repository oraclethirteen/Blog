using AutoMapper;
using Blog.DAL.Models;
using Blog.DAL.Repository;
using Blog.DAL.UoW;
using Blog.Models;
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
        private IMapper _mapper;
        private IUnitOfWork _UoW;
        private Repository<Article> _articleRepository;

        public ArticleController(IMapper mapper, IUnitOfWork UoW)
        {
            _mapper = mapper;
            _UoW = UoW;
            _articleRepository = (Repository<Article>)_UoW.GetRepository<Article>();
        }

        [Route("AddArticle")]
        [HttpPost]
        public async Task<string> Add(ArticleEditViewModel newArticle, int userId)
        {
            if (ModelState.IsValid)
            {
                var article = _mapper.Map<Article>(newArticle);

                article.Date = DateTime.Now;
                article.UserId = userId;

                await _articleRepository.Create(article);
                return "Действие выполнено успешно";
            }
            return string.Join("\r\n", ModelState.Values.SelectMany(v => v.Errors));
        }

        [Authorize]
        [HttpGet]
        [Route("Article")]
        public async Task<ArticleViewModel> GetArticle(int articleId)
        {
            ArticleViewModel resultArticle = new ArticleViewModel();

            Article article = await _articleRepository.Get(articleId);

            return _mapper.Map<ArticleViewModel>(article);
        }

        [Authorize]
        [HttpGet]
        [Route("ArticleList")]
        public List<ArticleViewModel> GetArticleList()
        {
            List<ArticleViewModel> resultArticleList = new List<ArticleViewModel>();

            var articleList = _articleRepository.GetAll();

            foreach (Article article in articleList)
            {
                resultArticleList.Add(_mapper.Map<ArticleViewModel>(article));
            }

            return resultArticleList;
        }

        [Authorize]
        [Route("EditArticle")]
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
            return "Пост успешно удалён";
        }
    }
}
