using AutoMapper;
using Blog.DAL.Models;
using Blog.DAL.Repository;
using Blog.DAL.UoW;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Blog.Models;
using Blog.BLL.Models;
using Blog.Models.User;
using Blog.Models.Role;
using Blog.Extensions;

namespace Blog.Controllers
{
    /// <summary>
    /// Класс контроллера, реализующий основные CRUD-операции пользователя
    /// </summary>
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private IMapper _mapper;
        private IUnitOfWork _UoW;
        private UserRepository _userRepository;
        private IRepository<UserRole> _userRoleRepository;
        private IRepository<Role> _roleRepository;

        public UserController(ILogger<UserController> logger,
            IMapper mapper,
            IUnitOfWork UoW)
        {
            _logger = logger;
            _mapper = mapper;
            _UoW = UoW;
            _userRepository = (UserRepository)_UoW.GetRepository<User>();
            _userRoleRepository = _UoW.GetRepository<UserRole>();
            _roleRepository = _UoW.GetRepository<Role>();
        }

        /// <summary>
        /// Запрос представления аутентификации
        /// </summary>
        /// <returns>Представление аунтификации</returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Authenticate()
        {
            return View();
        }

        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        /// <param name="model">Модель представления пользователя</param>
        /// <returns>В случае некорректного ввода данных возвращает обратно представление модели,
        /// в противном случае представление списка статей</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Authenticate(UserAuthenticateViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = _userRepository.GetByLogin(model.Login);

                if (user != null && user.Password == model.Password)
                {
                    UserDomain userDomain = UserDomain.CreateUserDomain(user);
                    await Authenticate(userDomain); // Аутентификация
                    return RedirectToAction("ArticleList", "Article");
                }
                ModelState.AddModelError("", "Некорректные логин и (или) пароль");
                _logger.LogInformation(ModelState.GetAllError());
            }
            else
            {
                _logger.LogInformation(ModelState.GetAllError());
            }

            return View(model);
        }

        private async Task Authenticate(UserDomain userDomain)
        {
            // Создание клейма для логина и ролей
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userDomain.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, userDomain.Roles.FirstOrDefault()?.Title),
                new Claim(ClaimTypes.NameIdentifier, userDomain.Id.ToString(), ClaimValueTypes.Integer),
            };

            // Создание объекта ClaimsIdentity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                 "AppCookie",
                 ClaimsIdentity.DefaultNameClaimType,
                 ClaimsIdentity.DefaultRoleClaimType);

            // Установка аутентификационных cookie
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            _logger.LogInformation($"Успешная аутентификация пользователя: {User.Identity.Name}");
        }

        /// <summary>
        /// Запрос представления для регистрации
        /// </summary>
        /// <returns>Представление аунтификации</returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Регистрация пользователя в приложении
        /// </summary>
        /// <param name="newUser">Модель представления нового пользователя</param>
        /// <returns>Вовзаращает представление списка статей,
        /// в противном случае (при неудачной регистрации) представление регистрации</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterViewModel newUser)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(newUser);

                UserRole userRole = new UserRole { User = user, RoleId = 3 /*ID роли пользованеля*/ };
                user.UserRoles.Add(userRole);

                // Добавление пользователя в БД
                await _userRepository.Create(user);
                await Authenticate(_mapper.Map<UserDomain>(user)); // Аутентификация

                return View("/Article/ArticleList");
            }
            else
            {
                _logger.LogInformation(ModelState.GetAllError());
            }

            return View(newUser);
        }

        /// <summary>
        /// Выход из приложения
        /// </summary>
        /// <returns>Представление аунтификации</returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation($"Пользователь {User.Identity.Name} вышел");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Authenticate");
        }

        /// <summary>
        /// Запрос представления редактирования пользователя
        /// </summary>
        /// <returns>Представление редактирования пользователя</returns>
        /// <param name="id">Идентификатор пользователя</param>
        [HttpGet]
        [Authorize(Roles = "Aдминистратор")]
        public async Task<IActionResult> EditUser(int id)
        {
            User user = await _userRepository.Get(id);
            List<Role> allRoles = await Task.FromResult(_roleRepository.GetAll().ToList());
            UserEditViewModel userEdit = _mapper.Map<UserEditViewModel>(user);
            userEdit.CheckRoles = allRoles.Select(r => new CheckRoleViewModel
            {
                Id = r.Id,
                Title = r.Title,
                Checked = user.UserRoles.Any(ur => ur.RoleId == r.Id)
            }).ToList();

            return View(userEdit);

        }

        /// <summary>
        /// Редактирования пользователя
        /// </summary>
        /// <param name="model">Моедль представления редактирования пользователя</param>
        /// <returns>Представление редактирования пользователя</returns>
        [Authorize(Roles = "Aдминистратор")]
        [HttpPut]
        public async Task<IActionResult> EditUser(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.Get(model.Id);

                await _userRepository.Update(user);
                _logger.LogInformation($"Пользователь {user.Login} был отредактирован пользователем {User.Identity.Name}");

                return View(user.Id);
            }
            else
            {
                _logger.LogInformation(ModelState.GetAllError());
            }
            return View(model);
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Представление списка пользователей</returns>
        [Authorize(Roles = "Aдминистратор")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int userId)
        {
            User user = await _userRepository.Get(userId);
            if (user is null)
            {
                ViewBag.Message = $"Пользователь (ID = {userId}) не найден";
                return View("UserList");
            }
            await _userRepository.Delete(user);
            _logger.LogInformation("Пользователь удалён");
            ViewBag.Message = $"Пользователь {user.Login} удалён";

            return View("UserList");
        }

        /// <summary>
        /// Запрос представления списка пользователей
        /// </summary>
        /// <returns>Представление списка пользователей</returns>
        [HttpGet]
        [Authorize(Roles = "Aдминистратор")]
        public async Task<IActionResult> UserList()
        {
            var userList = await Task.FromResult(_userRepository.GetAll());
            List<UserViewModel> resultUserList = _mapper.Map<List<UserViewModel>>(userList);
            _logger.LogInformation($"Пользователь {User.Identity.Name} запросил список пользователей.");

            return View(resultUserList);
        }

        /// <summary>
        /// Запрос представления пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns>Представление пользователя</returns>
        [Authorize(Roles = "Aдминистратор")]
        [HttpGet]
        public async Task<IActionResult> UserView(int id)
        {
            var user = await _userRepository.Get(id);
            if (user == null)
            {
                _logger.LogInformation($"Пользователь (ID = {id}) не найден");
                ViewBag.Message = $"Пользователь (ID = {id}) не найден";
                UserViewModel resultUser = new UserViewModel();
                return View(resultUser);
            }
            return View(_mapper.Map<UserViewModel>(user));
        }
    }
}
