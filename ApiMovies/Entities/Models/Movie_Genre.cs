using System;

namespace ApiMovies.Entities.Models
{
    public class Movie_Genre
    {
        public DateTime Created { get; set; }
        public int MovieId { get; set; }
        public  Movie Movie { get; set; }
        public int GenreId { get; set; }
        public  Genre Genre { get; set; }       
    }
}
