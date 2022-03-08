using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Entities.DTO
{
    public class MovieDTO
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 75)]
        [Required]
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Trailer { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }
        public DateTime Created { get; set; }
        public List<GenreDTO> Movie_Genres { get; set; }
        public List<ActorDTO> Movie_Actors { get; set; }
    }
}
