using ApiMovies.Database.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Entities.Models
{
    public class Movie : IEntityBase
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 75)]
        [Required]
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Trailer { get; set; }        
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }
        public List<Movie_Genre> Movie_Genres { get; set; }        
        public List<Movie_Actor> Movie_Actors { get; set; }
    }
}
