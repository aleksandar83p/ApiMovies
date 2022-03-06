using ApiMovies.Entities.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiMovies.Database.Services.Interface
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> RegisterAsync(UserCredentials userCredentials);
        Task<AuthenticationResponse> LoginAsync(UserCredentials userCredentials);
        Task MakeAdminAsync(AdminDTO adminDTO);
        Task RemoveAdminAsync(AdminDTO adminDTO);
        Task<List<UserDTO>> GetListOfUsersAsync(string sortBy, string searchString, int? pageNumber);

    }
}
