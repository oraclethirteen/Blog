using AutoMapper;
using Blog.DAL.Models;
using Blog.DAL.Repository;
using Blog.DAL.UoW;
using Blog.Extensions;
using Blog.Models.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    /// <summary>
    /// Класс контроллера, реализующий основные CRUD-операции тега
    /// </summary>
    public class TagController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private IMapper _mapper;
        private IUnitOfWork _UoW;
        private Repository<Tag> _tagRepository;

        public TagController(IMapper mapper, IUnitOfWork UoW, ILogger<UserController> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _UoW = UoW;
            _tagRepository = (Repository<Tag>)_UoW.GetRepository<Tag>();
        }

        /// <summary>
        /// Запрос представления добавления тега
        /// </summary>
        /// <returns>Представление добавления тега</returns>
        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// Добавление тега
        /// </summary>
        /// <param name="newTag"></param>
        /// <returns>Представление списка тегов</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddTag(TagEditViewModel newTag)
        {
            if (ModelState.IsValid)
            {
                var tag = _mapper.Map<Tag>(newTag);

                await _tagRepository.Create(tag);
                _logger.LogInformation($"Пользователь {User.Identity.Name} добавил тег {tag.Title}");

                return RedirectToAction("TagList");
            }
            else
            {
                _logger.LogInformation(ModelState.GetAllError());
            }

            return View(newTag);
        }

        /// <summary>
        /// Запрос представления редактирования тега
        /// </summary>
        /// <param name="id">Идентификатор тега</param>
        /// <returns>Представление редактирования тэга</returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditTag(int id)
        {
            Tag tag = await _tagRepository.Get(id);
            TagEditViewModel tagEdit = _mapper.Map<TagEditViewModel>(tag);

            return View(tagEdit);
        }

        /// <summary>
        /// Редактирование тега
        /// </summary>
        /// <param name="model">Модель представления редактирования тега</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> EditTag(TagEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tag = await _tagRepository.Get(model.Id);

                await _tagRepository.Update(tag);
                _logger.LogInformation($"Пользователь {User.Identity.Name} отредактировал тег (ID = {tag.Id}) {tag.Title}");

                return View("TagList");
            }
            else
            {
                _logger.LogInformation(ModelState.GetAllError());
            }

            return View(model);

        }

        /// <summary>
        /// Удаление тега
        /// </summary>
        /// <param name="Id">Идентификатор тега</param>
        /// <returns>Представление списка тегов</returns>
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(int tagId)
        {
            Tag tag = await _tagRepository.Get(tagId);
            if (tag is null)
            {
                return View("NotFound");
            }

            await _tagRepository.Delete(tag);
            _logger.LogInformation($"Пользователь {User.Identity.Name} удалил тег {tag.Title}");
            return RedirectToAction("TagList");

        }

        /// <summary>
        /// Получить представление списка тегов
        /// </summary>
        /// <returns>Представление списка тегов</returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> TagList()
        {
            var tagList = await Task.FromResult(_tagRepository.GetAll());
            List<TagViewModel> resultTagList = _mapper.Map<List<TagViewModel>>(tagList);

            return View(resultTagList);
        }

        /// <summary>
        /// Получение тега по ID
        /// </summary>
        /// <param name="id">Идентификатор тега</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ViewTag(int id)
        {

            Tag tag = await _tagRepository.Get(id);
            if (tag == null)
            {
                _logger.LogInformation($"Статья (ID = {id}) не найдена");
                return View("NotFound");
            }

            return View(_mapper.Map<TagViewModel>(tag));
        }
    }
}
