using Blog.BLL.Models;
using Blog.BLL.Response;
using Blog.DAL.Models;
using Blog.DAL.Repository;
using Blog.DAL.UoW;

namespace Blog.BLL.Services
{
    public class TagService : ITagService
    {
        private IUnitOfWork _UoW;
        private Repository<Tag> _tagRepository;

        public TagService(IUnitOfWork UoW)
        {
            _UoW = UoW;
            _tagRepository = (Repository<Tag>)_UoW.GetRepository<Tag>();
        }

        public async Task<EntityBaseResponse<TagDomain>> Get(int id)
        {
            Tag tag = await _tagRepository.Get(id);

            if (tag != null)
            {
                await _tagRepository.LoadNavigateProperty(tag);
                return new EntityBaseResponse<TagDomain>(Helper.Mapper.Map<TagDomain>(tag));
            }
            else
            {
                return new EntityBaseResponse<TagDomain>($"Тег (ID = {id}) не найден");
            }
        }

        public async Task<EntityBaseResponse<TagDomain>> Add(TagDomain tagDomain)
        {
            Tag newTag = Helper.Mapper.Map<Tag>(tagDomain);
            await _tagRepository.Create(newTag);

            return new EntityBaseResponse<TagDomain>(Helper.Mapper.Map<TagDomain>(newTag));
        }

        public async Task<EntityBaseResponse<TagDomain>> Update(TagDomain tagDomain)
        {
            Tag tag = await _tagRepository.Get(tagDomain.Id);

            if (tag != null)
            {
                await _tagRepository.LoadNavigateProperty(tag);

                tag = Helper.Mapper.Map(tagDomain, tag);
                await _tagRepository.Update(tag);

                return new EntityBaseResponse<TagDomain>(Helper.Mapper.Map<TagDomain>(tag));
            }
            else
            {
                return new EntityBaseResponse<TagDomain>(false, $"Тег '{tagDomain.Title}' (ID = {tagDomain.Id}) не найден");
            }
        }

        public async Task<EntityBaseResponse<TagDomain>> Delete(TagDomain tagDomain)
        {
            Tag tag = await _tagRepository.Get(tagDomain.Id);

            if (tag != null)
            {
                await _tagRepository.Delete(tag);
                return new EntityBaseResponse<TagDomain>(true, $"Тег '{tag.Title}' (ID = {tag.Id}) успешно удалён", tagDomain);
            }
            else
            {
                return new EntityBaseResponse<TagDomain>(false, $"Тег '{tagDomain.Title}' (ID = {tagDomain.Id}) не найден");
            }
        }

        public EntityBaseResponse<IEnumerable<TagDomain>> GetAll()
        {
            var tagList = _tagRepository.GetAll(t => t.ArticleTags);
            return new EntityBaseResponse<IEnumerable<TagDomain>>(Helper.Mapper.Map<IEnumerable<TagDomain>>(tagList));
        }
    }
}
