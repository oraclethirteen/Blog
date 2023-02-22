using AutoMapper;
using Blog.DAL.Models;
using Blog.BLL.Models;

namespace Blog.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Article
            CreateMap<Article, ArticleDomain>()
                .ForMember(dst => dst.Tags, src => src.MapFrom(m => m.ArticleTags.Select(pt => pt.Tag)));
            CreateMap<ArticleDomain, Article>()
                .ForMember(dst => dst.ArticleTags, src => src.MapFrom(m => m.Tags.Select(t => new ArticleTag { ArticleId = m.Id, TagId = t.Id })));

            // Comment
            CreateMap<Comment, CommentDomain>();
            CreateMap<CommentDomain, Comment>()
                .ForMember(dst => dst.Article, src => src.MapFrom(m => m.Article))
                .ForMember(dst => dst.User, src => src.MapFrom(m => m.User));

            // Role
            CreateMap<Role, RoleDomain>()
                .ForMember(r => r.Users, x => x.MapFrom(m => m.UserRoles.Select(ur => ur.User)));
            CreateMap<RoleDomain, Role>()
                .ForMember(u => u.UserRoles, x => x.MapFrom(m => m.Users.Select(r => new UserRole { UserId = r.Id, User = null, RoleId = r.Id, Role = null })));

            // Tag
            CreateMap<Tag, TagDomain>()
                .ForMember(dst => dst.Articles, x => x.MapFrom(m => m.ArticleTags.Select(ur => ur.Article)));
            CreateMap<TagDomain, Tag>()
                .ForMember(dst => dst.ArticleTags, x => x.MapFrom(m => m.Articles.Select(src => new ArticleTag { ArticleId = src.Id, TagId = m.Id })));

            // User
            CreateMap<User, UserDomain>()
                .ForMember(u => u.Roles, x => x.MapFrom(m => m.UserRoles.Select(ur => ur.Role)));
            CreateMap<UserDomain, User>()
                .ForMember(u => u.UserRoles, x => x.MapFrom(m => m.Roles.Select(r => new UserRole { UserId = m.Id, User = null, RoleId = r.Id, Role = null })));
        }
    }
}
