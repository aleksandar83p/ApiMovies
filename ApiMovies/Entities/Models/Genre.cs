using ApiMovies.Validations;
using System;
using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Entities.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [FirstLetterUppercase]
        public string Name { get; set; }
        public DateTime Created { get; set; }        
    }
}
