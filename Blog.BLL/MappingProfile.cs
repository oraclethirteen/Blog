using AutoMapper;
using Blog.BLL.Models;
using Blog.DAL.Models;

namespace Blog.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDomain>()
                .ForMember(u => u.Roles, x => x.MapFrom(m => m.UserRoles.Select(r => r.Role)));

            CreateMap<Tag, TagDomain>()
                .ForMember(u => u.Articles, x => x.MapFrom(m => m.ArticleTags.Select(r => r.Article)));
        }
    }
}
