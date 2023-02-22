using Blog.BLL.Models;
using Blog.BLL.Response;

namespace Blog.BLL.Services
{
    public interface ITagService
    {
        Task<EntityBaseResponse<TagDomain>> Add(TagDomain tagDomain);

        Task<EntityBaseResponse<TagDomain>> Delete(TagDomain tagDomain);

        Task<EntityBaseResponse<TagDomain>> Update(TagDomain tagDomain);

        Task<EntityBaseResponse<TagDomain>> Get(int id);

        EntityBaseResponse<IEnumerable<TagDomain>> GetAll();
    }
}
