using ApiMovies.Entities.DTO;
using ApiMovies.Entities.Models;
using AutoMapper;

namespace ApiMovies.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ActorDTO, Actor>()
                .ReverseMap();

            CreateMap<ActorCreationDTO, Actor>()
                .ForMember(x => x.Picture, options => options.Ignore());

            CreateMap<ActorUpdateDTO, Actor>()                
                .ForMember(x => x.Picture, options => options.Ignore());

            CreateMap<GenreDTO, Genre>().ReverseMap();

            CreateMap<GenreCreationDTO, Genre>();

            CreateMap<MovieDTO, Movie>().ReverseMap();

            CreateMap<MovieCreationDTO, Movie>()
                .ForMember(x => x.Poster, options => options.Ignore());

            CreateMap<MovieUpdateDTO, Movie>()
                .ForMember(x => x.Poster, options => options.Ignore());

            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
    }
}
