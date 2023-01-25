using AutoMapper;

namespace Blog.BLL
{
    public static class Helper
    {
        static Mapper _mapper = new Mapper(new MapperConfiguration(r => r.AddProfile(new MappingProfile())));

        public static Mapper Mapper { get { return _mapper; } }
    }
}
