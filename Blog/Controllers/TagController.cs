using AutoMapper;
using Blog.DAL.Models;
using Blog.DAL.Repository;
using Blog.DAL.UoW;
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
        private IMapper _mapper;
        private IUnitOfWork _UoW;
        private Repository<Tag> _tagRepository;

        public TagController(IMapper mapper, IUnitOfWork UoW)
        {
            _mapper = mapper;
            _UoW = UoW;
            _tagRepository = (Repository<Tag>)_UoW.GetRepository<Tag>();
        }

        [AllowAnonymous]
        [Route("AddTag")]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [Route("AddTag")]
        [HttpPost]
        public async Task<string> AddTag(TagEditViewModel newTag, int userId)
        {
            if (ModelState.IsValid)
            {
                var tag = _mapper.Map<Tag>(newTag);


                await _tagRepository.Create(tag);
                return "Действие выполнено успешно";
            }
            return string.Join("\r\n", ModelState.Values.SelectMany(v => v.Errors));
        }

        [Authorize]
        [HttpGet]
        [Route("Tag")]
        public async Task<TagViewModel> GetTag(int tagId)
        {
            TagViewModel resultTag = new TagViewModel();

            Tag tag = await _tagRepository.Get(tagId);

            return _mapper.Map<TagViewModel>(tag);
        }

        [HttpGet]
        public async Task<IActionResult> TagList()
        {
            var tagList = await Task.FromResult(_tagRepository.GetAll());
            List<TagViewModel> resultTagList = _mapper.Map<List<TagViewModel>>(tagList);

            return View(resultTagList);
        }

        [HttpGet]
        public async Task<IActionResult> EditTag(int id)
        {
            Tag tag = await _tagRepository.Get(id);
            TagEditViewModel tagEdit = _mapper.Map<TagEditViewModel>(tag);

            return View(tagEdit);
        }


        [Authorize]
        [Route("EditTag")]
        [HttpPut]
        public async Task<string> Update(TagEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tag = await _tagRepository.Get(model.Id);

                await _tagRepository.Update(tag);

                return "Действие выполнено успешно";
            }
            else
            {
                return string.Join("\r\n", ModelState.Values.SelectMany(v => v.Errors));
            }
        }


        [Authorize]
        [Route("DeleteTag")]
        [HttpDelete]
        public async Task<string> Delete(int tagId)
        {
            Tag tag = await _tagRepository.Get(tagId);
            if (tag is null)
            {
                return $"Тег (ID = {tagId}) не найден";
            }

            await _tagRepository.Delete(tag);
            return "Тег успешно удалён";
        } 
    }
}
