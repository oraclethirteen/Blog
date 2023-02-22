using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blog.API.Extensions;
using Blog.BLL.Models;
using Blog.BLL.Response;
using Blog.BLL.Services;
using Blog.DAL.UoW;
using System.Security.Claims;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private IUnitOfWork _UoW;
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        /// <param name="model"> Модель аутентификации </param>
        /// <returns> EntityBaseResponse<UserDomain> </returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public async Task<EntityBaseResponse<UserDomain>> Authenticate([FromBody] UserAuthenticateDomain model)
        {
            EntityBaseResponse<UserDomain> userResponse = null;
            if (ModelState.IsValid)
            {
                userResponse = _userService.GetByLogin(model.Login);

                if (userResponse.Success)
                {
                    UserDomain userDomain = userResponse.Entity;
                    await Authenticate(userDomain);
                    userResponse.Message = "Аутентификация прошла успешно";
                }
            }
            else
            {
                userResponse = new EntityBaseResponse<UserDomain>("Model Error", ModelState.GetErrorMessages());
            }
            return userResponse;
        }

        private async Task Authenticate(UserDomain userDomain)
        {
            // Создание клаймов логина и ролей
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

        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="newUser"> Модель UserDomain нового пользователя </param>
        /// <returns> Task<EntityBaseResponse<UserDomain>>, результат регистрации </returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public async Task<EntityBaseResponse<UserDomain>> Register([FromBody] UserDomain newUser)
        {
            if (ModelState.IsValid)
            {
                // Добавление пользователя в БД
                var resultRegister = await _userService.Register(newUser);
                await Authenticate(resultRegister.Entity); // Аутентификация
                return resultRegister;
            }
            else
            {
                return new EntityBaseResponse<UserDomain>("Model Error", ModelState.GetErrorMessages());
            }
        }

        /// <summary>
        /// Выход из приложения
        /// </summary>
        /// <returns> View аунтентификации </returns>
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<BaseResponse> Logout()
        {
            string message = ($"Пользователь {User.Identity.Name} вышел");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return new EntityBaseResponse<UserDomain>(true, message);
        }

        /// <summary>
        /// Редактирование пользователя
        /// </summary>
        /// <param name="id"> Идентификатор редактируемого пользователя </param>
        /// <param name="model"> Модель UserDomain редактируемого пользователя </param>
        /// <returns> Task<EntityBaseResponse<UserDomain>>, результат редактирования </returns>
        [HttpPut("[action]/{id}")]
        [Authorize(Roles = "Aдминистратор")]
        public async Task<EntityBaseResponse<UserDomain>> EditUser(int id, [FromBody] UserDomain model)
        {
            if (ModelState.IsValid)
            {
                EntityBaseResponse<UserDomain> userResponse = await _userService.Update(model);
                return userResponse;
            }
            else
            {
                return new EntityBaseResponse<UserDomain>("Model Error", ModelState.GetErrorMessages());
            }
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="id"> Идентификатор пользователя </param>
        /// <returns> BaseResponse, результат удаления </returns>
        [HttpDelete("[action]/{id}")]
        [Authorize(Roles = "Aдминистратор")]
        public async Task<BaseResponse> DeleteUser(int id)
        {
            EntityBaseResponse<UserDomain> userResponse = await _userService.Get(id);
            if (!userResponse.Success)
            {
                return userResponse;
            }
            userResponse = await _userService.Delete(userResponse.Entity);
            return userResponse;
        }

        /// <summary>
        /// Получение списка пользоватей
        /// </summary>
        /// <returns> EntityBaseResponse<IEnumerable<UserDomain>>, результат выполнения </returns>
        [HttpGet("[action]")]
        //[Authorize(Roles = "Aдминистратор")]
        public async Task<EntityBaseResponse<IEnumerable<UserDomain>>> UserList()
        {
            var userList = await Task.FromResult(_userService.GetAll());

            return userList;
        }

        /// <summary>
        /// Получение пользователя
        /// </summary>
        /// <param name="id"> Идентификатор пользователя </param>
        /// <returns> UserDomain, модель пользователя </returns>
        [Authorize(Roles = "Aдминистратор")]
        [HttpGet("[action]")]
        public async Task<EntityBaseResponse<UserDomain>> GetUser(int id)
        {
            var userResponse = await _userService.Get(id);
            return userResponse;
        }
    }
}
