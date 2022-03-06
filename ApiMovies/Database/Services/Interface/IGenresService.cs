using ApiMovies.Entities.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiMovies.Database.Services.Interface
{
    public interface IGenresService
    {
        Task<List<GenreDTO>> GetAllGenresAsync(string sortBy, string searchString, int? pageNumber);
        Task<GenreDTO> GetGenreByIdAsync(int id);
        Task AddGenreAsync(GenreCreationDTO genreCreationDTO);
        Task UpdateGenreAsync(int id, GenreDTO genreUpdateDTO);
        Task DeleteGenreAsync(int id);
    }
}
