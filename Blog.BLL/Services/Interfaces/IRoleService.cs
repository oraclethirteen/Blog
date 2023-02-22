using Blog.BLL.Models;
using Blog.BLL.Response;

namespace Blog.BLL.Services
{
    public interface IRoleService
    {
        Task<EntityBaseResponse<RoleDomain>> Add(RoleDomain roleDomain);

        Task<EntityBaseResponse<RoleDomain>> Delete(RoleDomain roleDomain);

        Task<EntityBaseResponse<RoleDomain>> Update(RoleDomain roleDomain);

        Task<EntityBaseResponse<RoleDomain>> Get(int id);

        EntityBaseResponse<IEnumerable<RoleDomain>> GetAll();
    }
}
