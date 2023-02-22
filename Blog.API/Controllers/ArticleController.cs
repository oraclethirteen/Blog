using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blog.API.Extensions;
using Blog.BLL.Models;
using Blog.BLL.Response;
using Blog.BLL.Services;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        /// <summary>
        /// Получение списка статей
        /// </summary>
        /// <returns> EntityBaseResponse<IEnumerable<ArticleDomain>>, результат выполнения </returns>
        [HttpGet("[action]")]
        public async Task<EntityBaseResponse<IEnumerable<ArticleDomain>>> ArticleList()
        {
            var result = await Task.FromResult(_articleService.GetAll());

            return result;
        }

        /// <summary>
        /// Получение статьи
        /// </summary>
        /// <param name="id"> Идентификатор статьи </param>
        /// <returns> ArticleDomain, модель статьи </returns>
        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<EntityBaseResponse<ArticleDomain>> GetArticle(int id)
        {
            var userResponse = await _articleService.Get(id);

            return userResponse;
        }

        /// <summary>
        /// Добавление статьи
        /// </summary>
        /// <param name="newArticle"> ArticleDomain, новая статья </param>
        /// <returns> EntityBaseResponse<ArticleDomain>, результат добавления статьи </returns>
        [HttpPost("[action]")]
        [Authorize]
        public async Task<EntityBaseResponse<ArticleDomain>> AddArticle([FromBody] ArticleDomain newArticle)
        {
            if (ModelState.IsValid)
            {
                newArticle.UserId = User.Identity.GeUsertId();

                EntityBaseResponse<ArticleDomain> result = await _articleService.Add(newArticle);

                return result;
            }
            else
            {
                return new EntityBaseResponse<ArticleDomain>("Model Error", ModelState.GetErrorMessages());
            }
        }

        /// <summary>
        /// Редактирование статьи
        /// </summary>
        /// <param name="model"> ArticleDomain, редактируемая статья </param>
        /// <returns> EntityBaseResponse<ArticleDomain>, результат редактирования </returns>
        [Authorize]
        [HttpPut("[action]/{id}")]
        public async Task<EntityBaseResponse<ArticleDomain>> EditArticle(int id, [FromBody] ArticleDomain model)
        {
            if (ModelState.IsValid)
            {
                var result = await _articleService.Update(model);

                return result;
            }
            else
            {
                return new EntityBaseResponse<ArticleDomain>("Model Error", ModelState.GetErrorMessages());
            }
        }

        /// <summary>
        /// Удаление статьи
        /// </summary>
        /// <param name="id"> Идентификатор статьи </param>
        /// <returns> EntityBaseResponse<ArticleDomain>, результат удаление </returns>
        [Authorize]
        [HttpDelete("[action]/{id}")]
        public async Task<EntityBaseResponse<ArticleDomain>> DeleteArticle(int id)
        {
            EntityBaseResponse<ArticleDomain> articleResponse = await _articleService.Get(id);
            if (!articleResponse.Success)
            {
                return articleResponse;
            }

            articleResponse = await _articleService.Delete(articleResponse.Entity);

            return articleResponse;
        }
    }
}
