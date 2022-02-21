using ApiMovies.Database.Base;
using ApiMovies.Entities.Models;

namespace ApiMovies.Database.Services
{
    public interface IActorsService : IEntityBaseRepository<Actor>
    {
    }
}