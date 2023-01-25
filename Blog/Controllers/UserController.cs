﻿using AutoMapper;
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

namespace Blog.Controllers
{
    /// <summary>
    /// Класс контроллера, реализующий основные CRUD-операции пользователя
    /// </summary>Ц
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
        [HttpGet]
        public IActionResult Authenticate()
        {
            return View();
        }

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
                    await Authenticate(userDomain);
                    return RedirectToAction("ArticleList", "Article");

                }
                ModelState.AddModelError("", "Некорректный логин и (или) пароль");
            }

            return View(model);
        }

        private async Task Authenticate(UserDomain userDomain)
        {
            // Создание клаймов логина и ролей
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userDomain.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, userDomain.Roles.FirstOrDefault()?.Title)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                "AppCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            // Установка куки аутентификации
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
        }

        [AllowAnonymous]
        [Route("Register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [AllowAnonymous]
        [Route("Register")]
        [HttpPost]
        public async Task<string> Register(UserRegisterViewModel newUser)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(newUser);

                UserRole userRole = new UserRole { User = user, RoleId = 3 };
                user.UserRoles.Add(userRole);

                await _userRepository.Create(user);

                return "Действие выполнено успешно";
            }
            return string.Join("\r\n", ModelState.Values.SelectMany(v => v.Errors));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
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

        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            var userList = await Task.FromResult(_userRepository.GetAll());
            List<UserViewModel> resultUserList = _mapper.Map<List<UserViewModel>>(userList);

            return View(resultUserList);
        }

        [HttpGet]
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
