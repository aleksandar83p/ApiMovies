using ApiMovies.Database.Base;
using ApiMovies.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Entities.Models
{
    public class Genre : IEntityBase
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [FirstLetterUppercase]
        public string Name { get; set; }
        public DateTime Created { get; set; }

        public List<Movie_Genre> Genre_Movies { get; set; }
    }
}
