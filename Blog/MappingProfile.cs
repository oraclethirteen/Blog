using AutoMapper;
using Blog.DAL.Models;
using Blog.Models;

namespace Blog
{
    /// <summary>
    /// Класс маппингов
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserEditViewModel>();
            CreateMap<User, UserViewModel>();

            CreateMap<Article, ArticleViewModel>();
            CreateMap<Article, ArticleEditViewModel>();

            CreateMap<Tag, TagViewModel>();
            CreateMap<Tag, TagEditViewModel>();

            CreateMap<Comment, CommentViewModel>();
            CreateMap<Comment, CommentEditViewModel>();
        }
    }
}
