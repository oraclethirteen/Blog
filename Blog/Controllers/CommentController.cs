using AutoMapper;
using Blog.DAL.Models;
using Blog.DAL.Repository;
using Blog.DAL.UoW;
using Blog.Models.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    /// <summary>
    /// Класс контроллера, реализующий основные CRUD-операции комментария
    /// </summary>
    public class CommentController : Controller
    {
        private IMapper _mapper;
        private IUnitOfWork _UoW;
        private Repository<Comment> _commentRepository;

        public CommentController(IMapper mapper, IUnitOfWork UoW)
        {
            _mapper = mapper;
            _UoW = UoW;
            _commentRepository = (Repository<Comment>)_UoW.GetRepository<Comment>();
        }

        [Route("AddComment")]
        [HttpPost]
        public async Task<string> Add(CommentEditViewModel newComment, int userId)
        {
            if (ModelState.IsValid)
            {
                var comment = _mapper.Map<Comment>(newComment);

                comment.Date = DateTime.Now;
                comment.UserId = userId;

                await _commentRepository.Create(comment);
                return "Действие выполнено успешно";
            }
            return string.Join("\r\n", ModelState.Values.SelectMany(v => v.Errors));
        }

        [Authorize]
        [HttpGet]
        public async Task<CommentViewModel> GetComment(int commentId)
        {
            CommentViewModel resultComment = new CommentViewModel();

            Comment comment = await _commentRepository.Get(commentId);

            return _mapper.Map<CommentViewModel>(comment);
        }

        [HttpGet]
        public async Task<IActionResult> CommentList()
        {
            var commentList = await Task.FromResult(_commentRepository.GetAll());
            List<CommentViewModel> resultCommetList = _mapper.Map<List<CommentViewModel>>(commentList);

            return View(resultCommetList);
        }

        [Authorize]
        [Route("EditComment")]
        [HttpPut]
        public async Task<string> Update(CommentEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var comment = await _commentRepository.Get(model.Id);

                await _commentRepository.Update(comment);

                return "Действие выполнено успешно";
            }
            else
            {
                return string.Join("\r\n", ModelState.Values.SelectMany(v => v.Errors));
            }
        }

        [Authorize]
        [Route("DeleteComment")]
        [HttpDelete]
        public async Task<string> Delete(int commentId)
        {
            Comment comment = await _commentRepository.Get(commentId);
            if (comment is null)
            {
                return $"Комментарий (ID = {commentId}) не найден";
            }

            await _commentRepository.Delete(comment);
            return "Комментарий успешно удалён";
        }
    }
}
