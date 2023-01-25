using AutoMapper;
using Blog.BLL.Models;
using Blog.DAL.Models;
using Blog.Models.Article;
using Blog.Models.Comment;
using Blog.Models.Role;
using Blog.Models.Tag;
using Blog.Models.User;

namespace Blog
{
    /// <summary>
    /// Класс, реализующий маппинги
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserEditViewModel>();
            CreateMap<User, UserViewModel>()
                .ForMember(dst => dst.Roles, src => src.MapFrom(c => c.UserRoles));

            CreateMap<Article, ArticleViewModel>()
                .ForMember(dst => dst.Tags, src => src.MapFrom(c => c.ArticleTags))
                .ForMember(dst => dst.Author, src => src.MapFrom(c => c.User.Email));
            CreateMap<Article, ArticleEditViewModel>();
            CreateMap<Article, ArticleCustomViewModel>();

            CreateMap<ArticleTag, TagViewModel>()
                .ForMember(dst => dst.Id, src => src.MapFrom(c => c.TagId))
                .ForMember(dst => dst.Title, src => src.MapFrom(c => c.Tag.Title));
            CreateMap<ArticleTag, TagCustomViewModel>()
                .ForMember(dst => dst.Id, src => src.MapFrom(c => c.TagId))
                .ForMember(dst => dst.Title, src => src.MapFrom(c => c.Tag.Title));

            CreateMap<Comment, CommentViewModel>()
                .ForMember(dst => dst.Author, src => src.MapFrom(c => c.User.Email));
            CreateMap<Comment, CommentEditViewModel>();

            CreateMap<Tag, TagViewModel>()
                .ForMember(dst => dst.Articles, srs => srs.MapFrom(c => c.ArticleTags));
            CreateMap<Tag, TagEditViewModel>();

            CreateMap<TagDomain, TagViewModel>();

            CreateMap<Role, RoleViewModel>();
            CreateMap<Role, RoleEditViewModel>();

            CreateMap<UserRole, RoleViewModel>()
                .ForMember(dst => dst.Id, src => src.MapFrom(c => c.RoleId))
                .ForMember(dst => dst.Title, src => src.MapFrom(c => c.Role.Title));
        }
    }
}
