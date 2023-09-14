using AutoMapper;

namespace RWA_API_project.Mappers
{
    public class MainMapper : Profile
    {
        public MainMapper()
        {
            CreateMap<Models.User, Models.UserVM>();
            CreateMap<Models.Video, Models.VideoVM>();
            CreateMap<Models.Tag, Models.TagVM>();
            CreateMap<Models.Genre, Models.GenreVM>();
        }
    }
}
