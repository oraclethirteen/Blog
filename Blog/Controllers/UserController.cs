using AutoMapper;
using Blog.DAL.Models;
using Blog.DAL.Repository;
using Blog.DAL.UoW;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using System.Security.Claims;
using Blog.Models;

namespace Blog.Controllers
{
    /// <summary>
    /// Класс контроллера, реализующий основные CRUD-операции пользователя
    /// </summary>
    [Authorize]
    public class UserController : Controller
    {
        private IMapper _mapper;
        private IUnitOfWork _UoW;
        private UserRepository _userRepository;
        private IRepository<UserRole> _userRoleRepository;
        private IRepository<Role> _roleRepository;

        public UserController(IMapper mapper, IUnitOfWork UoW)
        {
            _mapper = mapper;
            _UoW = UoW;
            _userRepository = (UserRepository)_UoW.GetRepository<User>();
            _userRoleRepository = _UoW.GetRepository<UserRole>();
            _roleRepository = _UoW.GetRepository<Role>();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Authenticate")]
        public async Task<UserViewModel> Authenticate(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("Запрос не корректен");
            }

            User user = _userRepository.GetByLogin(login);

            if (user is null)
            {
                throw new AuthenticationException("Пользователь не найден");
            }

            if (user.Password != password)
            {
                throw new AuthenticationException("Введенный пароль не корректен");
            }

            _userRoleRepository.GetAll();
            _roleRepository.GetAll();

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.UserRoles.FirstOrDefault().Role.Title)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                "AppCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return _mapper.Map<UserViewModel>(user);
        }

        [Route("Register")]
        [HttpPost]
        public async Task<string> Register(UserRegisterViewModel newUser)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(newUser);

                UserRole userRole = new UserRole { User = user, RoleId = 3 /*Id роли пользованель*/ };
                user.UserRoles.Add(userRole);

                await _userRepository.Create(user);

                return "Действие выполнено успешно";
            }
            return string.Join("\r\n", ModelState.Values.SelectMany(v => v.Errors));
        }

        [Authorize]
        [HttpGet]
        [Route("User")]
        public async Task<UserViewModel> GetUser(int userId)
        {
            UserViewModel resultUser = new UserViewModel();

            User user = await _userRepository.Get(userId);

            return _mapper.Map<UserViewModel>(user);
        }

        [Authorize(Roles = "Aдминистратор")]
        [HttpGet]
        [Route("UserList")]
        public List<UserViewModel> GetUserList()
        {
            List<UserViewModel> resultUserList = new List<UserViewModel>();

            var userList = _userRepository.GetAll();

            foreach (User user in userList)
            {
                resultUserList.Add(_mapper.Map<UserViewModel>(user));
            }

            return resultUserList;
        }

        [Authorize]
        [Route("EditUser")]
        [HttpPut]
        public async Task<string> Update(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.Get(model.Id);

                await _userRepository.Update(user);

                return "Действие выполнено успешно";
            }
            else
            {
                return string.Join("\r\n", ModelState.Values.SelectMany(v => v.Errors));
            }
        }

        [Authorize]
        [Route("DeleteUser")]
        [HttpDelete]
        public async Task<string> Delete(int userId)
        {
            User user = await _userRepository.Get(userId);
            if (user is null)
            {
                return $"Пользователь (ID = {userId}) не найден";
            }

            await _userRepository.Delete(user);
            return "Пользователь успешно удалён";
        }  
    }
}
