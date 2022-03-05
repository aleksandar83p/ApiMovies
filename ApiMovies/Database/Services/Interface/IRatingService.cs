using ApiMovies.Entities.DTO;
using System.Threading.Tasks;

namespace ApiMovies.Database.Services.Interface
{
    public interface IRatingService
    {
        Task AddRatingAsync(string email, RatingCreateDTO ratingCreateDTO);
    }
}
