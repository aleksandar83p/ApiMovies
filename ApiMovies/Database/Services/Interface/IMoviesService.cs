using ApiMovies.Entities.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiMovies.Database.Services.Interface
{
    public interface IMoviesService
    {
        Task<List<MovieDTO>> GetAllMoviesAsync();
        Task<MovieDTO> GetMoviesByIdAsync(int id);
        Task AddMoviesAsync(MovieCreationDTO movieCreationDTO);
        Task UpdateMoviesAsync(int id, MovieUpdateDTO movieDtoUpdate);
        Task DeleteMoviesAsync(int id);
    }
}
