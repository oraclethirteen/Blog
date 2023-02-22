using Blog.BLL.Models;
using Blog.BLL.Response;

namespace Blog.BLL.Services
{
    public interface IArticleService
    {
        Task<EntityBaseResponse<ArticleDomain>> Add(ArticleDomain articleDomain);

        Task<EntityBaseResponse<ArticleDomain>> Delete(ArticleDomain articleDomain);

        Task<EntityBaseResponse<ArticleDomain>> Update(ArticleDomain articleDomain);

        Task<EntityBaseResponse<ArticleDomain>> Get(int id);

        EntityBaseResponse<IEnumerable<ArticleDomain>> GetAll();

        IEnumerable<TagDomain> GetAllTags();
    }
}
