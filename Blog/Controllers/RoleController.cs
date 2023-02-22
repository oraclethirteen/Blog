using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Blog.Models;
using Blog.Extensions;
using Blog.BLL.Services;
using Blog.BLL.Models;
using Blog.BLL.Response;

namespace Blog.Controllers
{
    // Класс контроллера, реализующий основные CRUD-операции роли
    public class RoleController : Controller
    {
        private readonly ILogger<RoleController> _logger;
        private IMapper _mapper;
        private IRoleService _roleService;

        public RoleController(IMapper mapper, IRoleService roleService, ILogger<RoleController> logger)
        {
            _mapper = mapper;
            _roleService = roleService;
            _logger = logger;
        }

        /// <summary>
        /// Запрос представления добавления роли
        /// </summary>
        /// <returns> View добавления роли </returns>
        [Authorize(Roles = "Aдминистратор")]
        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }

        /// <summary>
        /// Добавление роли
        /// </summary>
        /// <param name="newRole"> Модель новой роли </param>
        /// <returns> View списка ролей </returns>
        [HttpPost]
        [Authorize(Roles = "Aдминистратор")]
        public async Task<IActionResult> AddRole(RoleEditViewModel newRole)
        {
            if (ModelState.IsValid)
            {
                var role = _mapper.Map<RoleDomain>(newRole);

                EntityBaseResponse<RoleDomain> result = await _roleService.Add(role);
                if (result.Success)
                {
                    _logger.LogInformation($"Пользователь {User.Identity.Name} добавил роль {role.Description}.");
                    return RedirectToAction("RoleList");
                }
            }
            else
            {
                _logger.LogInformation(ModelState.GetAllError());
            }

            return View(newRole);
        }

        /// <summary>
        /// Запрос представления редактирования роли
        /// </summary>
        /// <param name="id"> Идентификатор роли </param>
        /// <returns> View редактирования роли </returns>
        [Authorize(Roles = "Aдминистратор")]
        [HttpGet]
        public async Task<IActionResult> EditRole(int id)
        {
            EntityBaseResponse<RoleDomain> role = await _roleService.Get(id);
            if (role.Success)
            {
                RoleEditViewModel roleEdit = _mapper.Map<RoleEditViewModel>(role.Entity);
                return View(roleEdit);
            }
            else
            {
                return View("NotFound");
            }
        }

        /// <summary>
        /// Редактирование роли
        /// </summary>
        /// <param name="model"> ViewModel редактирования роли </param>
        /// <returns> View редактирования роли </returns>
        [Authorize(Roles = "Aдминистратор")]
        [HttpPost]
        public async Task<IActionResult> EditRole(RoleEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = _mapper.Map<RoleDomain>(model);
                await _roleService.Update(role);
                _logger.LogInformation($"Пользователь {User.Identity.Name} отредактировал роль id = {role.Id} {role.Description}");

                return RedirectToAction("RoleList");
            }
            else
            {
                _logger.LogInformation(ModelState.GetAllError());
            }

            return View(model);
        }

        /// <summary>
        /// Удаление роли
        /// </summary>
        /// <param name="id"> Идентификатор роли </param>
        /// <returns> View списка ролей </returns>
        [Authorize(Roles = "Aдминистратор")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            EntityBaseResponse<RoleDomain> role = await _roleService.Get(id);
            if (!role.Success)
            {
                return View("NotFound");
            }

            role = await _roleService.Delete(role.Entity);
            if (role.Success)
            {
                _logger.LogInformation($"Пользователь {User.Identity.Name} удалил роль {role.Entity.Description}.");
            }

            return RedirectToAction("RoleList");
        }

        /// <summary>
        /// Получение View списка ролей
        /// </summary>
        /// <returns> View списка ролей </returns>
        [Authorize(Roles = "Aдминистратор")]
        [HttpGet]
        public async Task<IActionResult> RoleList()
        {
            var roleList = await Task.FromResult(_roleService.GetAll());
            List<RoleViewModel> resultRoleList = _mapper.Map<List<RoleViewModel>>(roleList.Entity);

            return View(resultRoleList);
        }

        /// <summary>
        /// Получение роли по ID
        /// </summary>
        /// <param name="id"> Идентификатор роли </param>
        /// <returns> View роли </returns>
        [Authorize(Roles = "Aдминистратор")]
        [HttpGet]
        public async Task<IActionResult> ViewRole(int id)
        {

            var roleResponse = await _roleService.Get(id);
            if (!roleResponse.Success)
            {
                _logger.LogInformation($"Роль с Id = {id} не найдена.");
                return View("NotFound");
            }

            return View(_mapper.Map<RoleViewModel>(roleResponse.Entity));

        }
    }
}
