using System;
using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Entities.Models
{
    public class Movie_Actor
    {
        [StringLength(maximumLength: 75)]
        public string CharacterName { get; set; }
        public int OrderInCharactersList { get; set; }
        public DateTime Created { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}
