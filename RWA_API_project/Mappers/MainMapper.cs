using AutoMapper;

namespace RWA_API_project.Mappers
{
    public class MainMapper : Profile
    {
        public MainMapper()
        {
            CreateMap<Models.User, Models.UserVM>().ReverseMap();
            CreateMap<Models.Video, Models.VideoVM>().ReverseMap();
            CreateMap<Models.Tag, Models.TagVM>().ReverseMap();
            CreateMap<Models.Genre, Models.GenreVM>().ReverseMap();
        }
    }
}
