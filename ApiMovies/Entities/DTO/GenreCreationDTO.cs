using ApiMovies.Validations;
using System;
using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Entities.DTO
{
    public class GenreCreationDTO
    {
        [Required]
        [StringLength(50)]
        [FirstLetterUppercase]
        public string Name { get; set; }       
    }
}
