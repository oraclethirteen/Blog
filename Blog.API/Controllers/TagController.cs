using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blog.API.Extensions;
using Blog.BLL.Models;
using Blog.BLL.Response;
using Blog.BLL.Services;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        /// <summary>
        /// Получение списка тегов
        /// </summary>
        /// <returns> EntityBaseResponse<IEnumerable<TagDomain>>, результат выполнения </returns>
        [Authorize]
        [HttpGet("[action]")]
        public async Task<EntityBaseResponse<IEnumerable<TagDomain>>> TagList()
        {
            var result = await Task.FromResult(_tagService.GetAll());
            return result;
        }

        /// <summary>
        /// Добавление тега
        /// </summary>
        /// <param name="newTag"> TagDomain, новый тег </param>
        /// <returns> EntityBaseResponse<TagDomain>, результат добавления </returns>
        [HttpPost("[action]")]
        [Authorize]
        public async Task<EntityBaseResponse<TagDomain>> AddTag([FromBody] TagDomain newTag)
        {
            if (ModelState.IsValid)
            {
                EntityBaseResponse<TagDomain> result = await _tagService.Add(newTag);
                return result;
            }
            else
            {
                return new EntityBaseResponse<TagDomain>("Model Error", ModelState.GetErrorMessages());
            }
        }

        /// <summary>
        /// Редактирование тега
        /// </summary>
        /// <param name="id"> Идентификатор редактируемого тега </param>
        /// <param name="model"> TagDomain, редактируемый тег </param>
        /// <returns> EntityBaseResponse<TagDomain>, результат редактирования </returns>
        [Authorize]
        [HttpPut("[action]/{id}")]
        public async Task<EntityBaseResponse<TagDomain>> EditTag(int id, [FromBody] TagDomain model)
        {
            if (ModelState.IsValid)
            {
                var result = await _tagService.Update(model);

                return result;
            }
            else
            {
                return new EntityBaseResponse<TagDomain>("Model Error", ModelState.GetErrorMessages());
            }
        }

        /// <summary>
        /// Удаление тега
        /// </summary>
        /// <param name="id"> Идентификатор тега </param>
        /// <returns> EntityBaseResponse<TagDomain>, результат удаления </returns>
        [Authorize]
        [HttpDelete("[action]/{id}")]
        public async Task<EntityBaseResponse<TagDomain>> DeleteTag(int id)
        {
            EntityBaseResponse<TagDomain> tagResponse = await _tagService.Get(id);
            if (!tagResponse.Success)
            {
                return tagResponse;
            }
            tagResponse = await _tagService.Delete(tagResponse.Entity);
            return tagResponse;
        }

        /// <summary>
        /// Получение тега
        /// </summary>
        /// <param name="id"> Идентификатор тега </param>
        /// <returns> TagDomain, модель тэга </returns>
        [Authorize]
        [HttpGet("[action]")]
        public async Task<EntityBaseResponse<TagDomain>> GetTag(int id)
        {
            var userResponse = await _tagService.Get(id);
            return userResponse;
        }
    }
}
