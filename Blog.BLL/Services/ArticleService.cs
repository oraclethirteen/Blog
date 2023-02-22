using Blog.BLL.Models;
using Blog.BLL.Response;
using Blog.DAL.Models;
using Blog.DAL.Repository;
using Blog.DAL.UoW;

namespace Blog.BLL.Services
{
    public class ArticleService : IArticleService
    {
        private IUnitOfWork _UoW;
        private Repository<Article> _articleRepository;
        private Repository<Tag> _tagRepository;

        public ArticleService(IUnitOfWork UoW)
        {
            _UoW = UoW;
            _articleRepository = (Repository<Article>)_UoW.GetRepository<Article>();
            _tagRepository = (Repository<Tag>)_UoW.GetRepository<Tag>();
        }

        public async Task<EntityBaseResponse<ArticleDomain>> Get(int id)
        {
            Article article = await _articleRepository.Get(id);

            if (article != null)
            {
                article.Views += 1;
                await _articleRepository.Update(article);
                await _articleRepository.LoadNavigateProperty(article);
                return new EntityBaseResponse<ArticleDomain>(Helper.Mapper.Map<ArticleDomain>(article));
            }
            else
            {
                return new EntityBaseResponse<ArticleDomain>($"Статья (ID = {id}) не найдена");
            }
        }

        public async Task<EntityBaseResponse<ArticleDomain>> Add(ArticleDomain articleDomain)
        {
            Article newArticle = Helper.Mapper.Map<Article>(articleDomain);
            newArticle.Date = DateTime.Now;
            await _articleRepository.Create(newArticle);

            return new EntityBaseResponse<ArticleDomain>(Helper.Mapper.Map<ArticleDomain>(newArticle));
        }

        public async Task<EntityBaseResponse<ArticleDomain>> Update(ArticleDomain articleDomain)
        {
            Article article = await _articleRepository.Get(articleDomain.Id);

            if (article != null)
            {
                await _articleRepository.LoadNavigateProperty(article);
                articleDomain.UserId = article.UserId;
                articleDomain.User = (Helper.Mapper.Map<UserDomain>(article.User));

                article = Helper.Mapper.Map(articleDomain, article);
                await _articleRepository.Update(article);

                return new EntityBaseResponse<ArticleDomain>(Helper.Mapper.Map<ArticleDomain>(article));
            }
            else
            {
                return new EntityBaseResponse<ArticleDomain>(false, $"Статья '{articleDomain.Title}' (ID = {articleDomain.Id}) не найдена");
            }
        }

        public async Task<EntityBaseResponse<ArticleDomain>> Delete(ArticleDomain articleDomain)
        {
            Article article = await _articleRepository.Get(articleDomain.Id);

            if (article != null)
            {
                await _articleRepository.Delete(article);
                return new EntityBaseResponse<ArticleDomain>(true, $"Статья '{article.Title}' (ID = {article.Id}) успешно удалена", articleDomain);
            }
            else
            {
                return new EntityBaseResponse<ArticleDomain>(false, $"Статья '{articleDomain.Title}' (ID = {articleDomain.Id}) не найдена");
            }
        }

        public EntityBaseResponse<IEnumerable<ArticleDomain>> GetAll()
        {
            var articleList = _articleRepository.GetAll(p => p.User, p => p.ArticleTags, p => p.Comments);
            return new EntityBaseResponse<IEnumerable<ArticleDomain>>(Helper.Mapper.Map<IEnumerable<ArticleDomain>>(articleList));
        }

        public IEnumerable<TagDomain> GetAllTags()
        {
            var tagList = _tagRepository.GetAll();
            return Helper.Mapper.Map<IEnumerable<TagDomain>>(tagList);
        }
    }
}
