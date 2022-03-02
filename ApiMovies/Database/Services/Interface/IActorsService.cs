using ApiMovies.Entities.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiMovies.Database.Services.Interface
{
    public interface IActorsService 
    {
        Task<List<ActorDTO>> GetAllActorsAsync();
        Task<ActorDTO> GetActorByIdAsync(int id);
        Task AddActorAsync(ActorCreationDTO actorCreationDTO);
        Task UpdateActorAsync(int id, ActorUpdateDTO actorUpdateDTO);
        Task DeleteActorAsync(int id);
    }
}