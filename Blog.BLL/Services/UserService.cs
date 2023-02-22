using Blog.BLL.Models;
using Blog.BLL.Response;
using Blog.DAL.Models;
using Blog.DAL.Repository;
using Blog.DAL.UoW;

namespace Blog.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork _UoW;
        private UserRepository _userRepository;
        private Repository<Role> _roleRepository;

        public UserService(IUnitOfWork UoW)
        {
            _UoW = UoW;
            _userRepository = (UserRepository)_UoW.GetRepository<User>();
            _roleRepository = (Repository<Role>)_UoW.GetRepository<Role>();
        }

        public async Task<EntityBaseResponse<UserDomain>> Register(UserDomain userDomain)
        {
            User newUser = Helper.Mapper.Map<User>(userDomain);

            // Добавление роли по умолчанию
            UserRole userRole = new UserRole { User = newUser, RoleId = 3 /*роль пользователя*/ };
            newUser.UserRoles.Add(userRole);

            await _userRepository.Create(newUser);

            return new EntityBaseResponse<UserDomain>(Helper.Mapper.Map<UserDomain>(newUser));
        }

        public EntityBaseResponse<UserDomain> Authenticate(UserAuthenticateDomain userAuthenticateDomain)
        {
            EntityBaseResponse<UserDomain> response = GetByLogin(userAuthenticateDomain.Login);

            if (response.Success)
            {
                return response;
            }
            else
            {
                response = new EntityBaseResponse<UserDomain>("Некорректные логин и (или) пароль");
                return response;
            }
        }

        public EntityBaseResponse<UserDomain> GetByLogin(string login)
        {
            User user = _userRepository.GetByLogin(login);
            if (user != null)
            {
                return new EntityBaseResponse<UserDomain>(Helper.Mapper.Map<UserDomain>(user));
            }
            else
            {
                return new EntityBaseResponse<UserDomain>($"Пользователь '{login}' не найден");
            }
        }

        public async Task<EntityBaseResponse<UserDomain>> Get(int id)
        {
            User user = await _userRepository.Get(id);

            if (user != null)
            {
                _userRepository.LoadNavigateProperty(user, u => u.UserRoles);
                return new EntityBaseResponse<UserDomain>(Helper.Mapper.Map<UserDomain>(user));
            }
            else
            {
                return new EntityBaseResponse<UserDomain>($"Пользователь (ID = {id}) не найден");
            }
        }

        public async Task<EntityBaseResponse<UserDomain>> Delete(UserDomain userDomain)
        {
            User user = await _userRepository.Get(userDomain.Id);

            if (user != null)
            {
                await _userRepository.Delete(user);
                return new EntityBaseResponse<UserDomain>(true, $"Пользователь '{user.LastName}' (ID = {user.Id}) успешно удалён", userDomain);
            }
            else
            {
                return new EntityBaseResponse<UserDomain>(false, $"Пользователь '{userDomain.LastName}' (ID = {userDomain.Id}) не найден");
            }
        }

        public async Task<EntityBaseResponse<UserDomain>> Update(UserDomain userDomain)
        {
            User user = await _userRepository.Get(userDomain.Id);

            if (user != null)
            {
                _userRepository.LoadNavigateProperty(user, u => u.UserRoles);
                user = Helper.Mapper.Map(userDomain, user);


                await _userRepository.Update(user);
                return new EntityBaseResponse<UserDomain>(Helper.Mapper.Map<UserDomain>(user));
            }
            else
            {
                return new EntityBaseResponse<UserDomain>(false, $"Пользователь {userDomain.LastName} (ID = {userDomain.Id}) не найден");
            }
        }

        public EntityBaseResponse<IEnumerable<UserDomain>> GetAll()
        {
            var userList = _userRepository.GetAll(u => u.UserRoles);
            return new EntityBaseResponse<IEnumerable<UserDomain>>(Helper.Mapper.Map<IEnumerable<UserDomain>>(userList));
        }

        public IEnumerable<RoleDomain> GetAllRoles()
        {
            var roleList = _roleRepository.GetAll();
            return Helper.Mapper.Map<IEnumerable<RoleDomain>>(roleList);
        }
    }
}
