using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Entities.DTO
{
    public class UserDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }        
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}
