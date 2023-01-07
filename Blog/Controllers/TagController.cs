using AutoMapper;
using Blog.DAL.Models;
using Blog.DAL.Repository;
using Blog.DAL.UoW;
using Blog.Models;
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

        [Route("AddTag")]
        [HttpPost]
        public async Task<string> Add(TagEditViewModel newTag, int userId)
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

        [Authorize]
        [HttpGet]
        [Route("TagList")]
        public List<TagViewModel> GetTagList()
        {
            List<TagViewModel> resultTagList = new List<TagViewModel>();

            var tagList = _tagRepository.GetAll();

            foreach (Tag tag in tagList)
            {
                resultTagList.Add(_mapper.Map<TagViewModel>(tag));
            }

            return resultTagList;
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
