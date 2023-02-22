using Blog.BLL.Models;
using Blog.BLL.Response;

namespace Blog.BLL.Services
{
    public interface ICommentService
    {
        Task<EntityBaseResponse<CommentDomain>> Add(CommentDomain commentDomain);

        Task<EntityBaseResponse<CommentDomain>> Delete(CommentDomain commentDomain);

        Task<EntityBaseResponse<CommentDomain>> Update(CommentDomain commentDomain);

        Task<EntityBaseResponse<CommentDomain>> Get(int id);

        Task<EntityBaseResponse<IEnumerable<CommentDomain>>> GetByArticle(int articleId);

        EntityBaseResponse<IEnumerable<CommentDomain>> GetAll();
    }
}
