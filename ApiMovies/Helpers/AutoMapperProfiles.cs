using ApiMovies.Entities.DTO;
using ApiMovies.Entities.Models;
using AutoMapper;

namespace ApiMovies.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ActorDTO, Actor>().ReverseMap();
            CreateMap<ActorCreationDTO, Actor>()
                .ForMember(x => x.Picture, options => options.Ignore());
            CreateMap<ActorUpdateDTO, Actor>().ReverseMap();

            CreateMap<GenreDTO, Genre>().ReverseMap();
            CreateMap<GenreCreationDTO, Genre>();
        }
    }
}
