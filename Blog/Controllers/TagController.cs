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
    // Класс контроллера, реализующий основные CRUD-операции тега
    public class TagController : Controller
    {
        private readonly ILogger<TagController> _logger;
        private IMapper _mapper;
        private ITagService _tagService;

        public TagController(IMapper mapper, ITagService tagService, ILogger<TagController> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _tagService = tagService;
        }

        /// <summary>
        /// Запрос представления добавления тега
        /// </summary>
        /// <returns> View добавления тега </returns>
        [Authorize]
        [Authorize]
        [HttpGet]
        public IActionResult AddTag()
        {
            return View();
        }

        /// <summary>
        /// Добавление тега
        /// </summary>
        /// <returns> View добавления тега </returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddTag(TagEditViewModel newTag)
        {
            if (ModelState.IsValid)
            {
                var tag = _mapper.Map<TagDomain>(newTag);

                EntityBaseResponse<TagDomain> result = await _tagService.Add(tag);
                if (result.Success)
                {
                    _logger.LogInformation($"Пользователь {User.Identity.Name} добавил тэг {tag.Title}.");
                    return RedirectToAction("TagList");
                }
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
        /// <param name="id"> Идентификатор тега </param>
        /// <returns> View редактирования тега returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditTag(int id)
        {
            EntityBaseResponse<TagDomain> tag = await _tagService.Get(id);
            if (tag.Success)
            {
                TagEditViewModel tagEdit = _mapper.Map<TagEditViewModel>(tag.Entity);
                return View(tagEdit);
            }
            else
            {
                return View("NotFound");
            }
        }

        /// <summary>
        /// Редактирование тега
        /// </summary>
        /// <param name="model"> ViewModel редактирования тега </param>
        /// <returns> View редактирования тега </returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditTag(TagEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tag = _mapper.Map<TagDomain>(model);
                await _tagService.Update(tag);
                _logger.LogInformation($"Пользователь {User.Identity.Name} отредактировал тэг id = {tag.Id} {tag.Title}");

                return RedirectToAction("TagList");
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
        /// <param name="id"> Идентификатор тега </param>
        /// <returns> View списка тегов</returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            EntityBaseResponse<TagDomain> tag = await _tagService.Get(id);
            if (!tag.Success)
            {
                return View("NotFound");
            }

            tag = await _tagService.Delete(tag.Entity);
            if (tag.Success)
            {
                _logger.LogInformation($"Пользователь {User.Identity.Name} удалил тэг {tag.Entity.Title}.");

            }

            return RedirectToAction("TagList");
        }

        /// <summary>
        /// Получение View списка тегов
        /// </summary>
        /// <returns> View списка тегов </returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> TagList()
        {
            var tagList = await Task.FromResult(_tagService.GetAll());
            List<TagViewModel> resultTagList = _mapper.Map<List<TagViewModel>>(tagList.Entity);

            return View(resultTagList);

        }

        /// <summary>
        /// Получение тега по ID
        /// </summary>
        /// <param name="id"> Идентификатор тега </param>
        /// <returns> View тега </returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ViewTag(int id)
        {
            var tagResponse = await _tagService.Get(id);

            if (!tagResponse.Success)
            {
                _logger.LogInformation(tagResponse.Message);
                return View("NotFound");
            }

            return View(_mapper.Map<TagViewModel>(tagResponse.Entity));
        }
    }
}
