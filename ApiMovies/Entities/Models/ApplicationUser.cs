using Microsoft.AspNetCore.Identity;

namespace ApiMovies.Entities.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
