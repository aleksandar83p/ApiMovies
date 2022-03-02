using ApiMovies.Validations;
using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Entities.DTO
{
    public class GenreDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [FirstLetterUppercase]
        public string Name { get; set; }        
    }
}
