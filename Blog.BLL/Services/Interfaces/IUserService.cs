﻿using Blog.BLL.Models;
using Blog.BLL.Response;

namespace Blog.BLL.Services
{
    public interface IUserService
    {
        Task<EntityBaseResponse<UserDomain>> Register(UserDomain userDomain);

        EntityBaseResponse<UserDomain> Authenticate(UserAuthenticateDomain userAuthenticateDomain);

        EntityBaseResponse<UserDomain> GetByLogin(string login);

        Task<EntityBaseResponse<UserDomain>> Get(int id);

        Task<EntityBaseResponse<UserDomain>> Delete(UserDomain userDomain);

        Task<EntityBaseResponse<UserDomain>> Update(UserDomain userDomain);

        EntityBaseResponse<IEnumerable<UserDomain>> GetAll();

        IEnumerable<RoleDomain> GetAllRoles();
    }
}
