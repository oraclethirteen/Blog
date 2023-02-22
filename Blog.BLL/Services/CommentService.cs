using Blog.BLL.Models;
using Blog.BLL.Response;
using Blog.DAL.Models;
using Blog.DAL.Repository;
using Blog.DAL.UoW;

namespace Blog.BLL.Services
{
    public class CommentService : ICommentService
    {
        private IUnitOfWork _UoW;
        private Repository<Comment> _commentRepository;
        private Repository<User> _userRepository;
        private Repository<Article> _articleRepository;

        public CommentService(IUnitOfWork UoW)
        {
            _UoW = UoW;
            _commentRepository = (Repository<Comment>)_UoW.GetRepository<Comment>();
            _articleRepository = (Repository<Article>)_UoW.GetRepository<Article>();
            _userRepository = (UserRepository)_UoW.GetRepository<User>();
        }

        public async Task<EntityBaseResponse<CommentDomain>> Get(int id)
        {
            Comment comment = await _commentRepository.Get(id);

            if (comment != null)
            {
                await _commentRepository.LoadNavigateProperty(comment);
                return new EntityBaseResponse<CommentDomain>(Helper.Mapper.Map<CommentDomain>(comment));
            }
            else
            {
                return new EntityBaseResponse<CommentDomain>($"Комментарий (ID = {id}) не найден");
            }
        }

        public async Task<EntityBaseResponse<CommentDomain>> Add(CommentDomain commentDomain)
        {
            Comment newComment = Helper.Mapper.Map<Comment>(commentDomain);
            newComment.Date = DateTime.Now;
            await _commentRepository.Create(newComment);

            return new EntityBaseResponse<CommentDomain>(Helper.Mapper.Map<CommentDomain>(newComment));
        }

        public async Task<EntityBaseResponse<CommentDomain>> Update(CommentDomain commentDomain)
        {
            Comment comment = await _commentRepository.Get(commentDomain.Id);

            if (comment != null)
            {

                await _commentRepository.LoadNavigateProperty(comment);
                comment = Helper.Mapper.Map(commentDomain, comment);
                comment.User = _userRepository.Get(comment.UserId).Result;
                comment.Article = _articleRepository.Get(comment.ArticleId).Result;
                await _commentRepository.Update(comment);

                return new EntityBaseResponse<CommentDomain>(Helper.Mapper.Map<CommentDomain>(comment));
            }
            else
            {
                return new EntityBaseResponse<CommentDomain>(false, $"Комментарий (ID = {commentDomain.Id}) не найден");
            }
        }

        public async Task<EntityBaseResponse<CommentDomain>> Delete(CommentDomain commentDomain)
        {
            Comment comment = await _commentRepository.Get(commentDomain.Id);

            if (comment != null)
            {
                await _commentRepository.Delete(comment);
                return new EntityBaseResponse<CommentDomain>(true, $"Комментарий (ID = {comment.Id}) успешно удалён", commentDomain);
            }
            else
            {
                return new EntityBaseResponse<CommentDomain>(false, $"Комментарий (ID = {commentDomain.Id}) не найден");
            }
        }

        public async Task<EntityBaseResponse<IEnumerable<CommentDomain>>> GetByArticle(int articleId)
        {
            var commentList = await _commentRepository.Get(p => p.ArticleId == articleId, c => c.OrderBy(c => c.Date));
            return new EntityBaseResponse<IEnumerable<CommentDomain>>(Helper.Mapper.Map<IEnumerable<CommentDomain>>(commentList));
        }

        public EntityBaseResponse<IEnumerable<CommentDomain>> GetAll()
        {
            var commentList = _commentRepository.GetAll(p => p.User, p => p.Article);
            return new EntityBaseResponse<IEnumerable<CommentDomain>>(Helper.Mapper.Map<IEnumerable<CommentDomain>>(commentList));
        }
    }
}
