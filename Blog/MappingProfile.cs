using AutoMapper;
using Blog.Models;
using Blog.BLL.Models;

namespace Blog
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Article
            CreateMap<ArticleDomain, ArticleViewModel>()
                .ForMember(dst => dst.Tags, src => src.MapFrom(c => c.Tags))
                .ForMember(dst => dst.Author, src => src.MapFrom(c => c.User.Email));

            CreateMap<ArticleDomain, ArticleEditViewModel>();
            CreateMap<ArticleDomain, ArticleCustomViewModel>();
            CreateMap<ArticleEditViewModel, ArticleDomain>()
                .ForMember(dst => dst.Tags,
                           src => src.MapFrom(c => c.CheckTags
                                                    .Where(t => t.Checked)
                                                    .Select(t => new TagDomain { Id = t.Id })));

            // Comment
            CreateMap<CommentDomain, CommentViewModel>()
                .ForMember(dst => dst.Author, src => src.MapFrom(c => c.User.Email));
            CreateMap<CommentDomain, CommentEditViewModel>();
            CreateMap<CommentEditViewModel, CommentDomain>();

            // Role
            CreateMap<RoleDomain, RoleViewModel>();
            CreateMap<RoleDomain, RoleEditViewModel>();
            CreateMap<RoleEditViewModel, RoleDomain>();

            // Tag
            CreateMap<TagDomain, TagViewModel>();
            CreateMap<TagDomain, TagCustomViewModel>();
            CreateMap<TagDomain, TagEditViewModel>();
            CreateMap<TagEditViewModel, TagDomain>();

            // User
            CreateMap<UserDomain, UserRegisterViewModel>();
            CreateMap<UserRegisterViewModel, UserDomain>();

            CreateMap<UserDomain, UserEditViewModel>();
            CreateMap<UserEditViewModel, UserDomain>()
                .ForMember(dst => dst.Roles,
                           src => src.MapFrom(c => c.CheckRoles
                                                    .Where(r => r.Checked)
                                                    .Select(r => new RoleDomain { Id = r.Id, Title = r.Title })));

            CreateMap<UserDomain, UserViewModel>()
                .ForMember(dst => dst.Roles, src => src.MapFrom(c => c.Roles)); 
        }
    }
}
