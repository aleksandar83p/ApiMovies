using System;
using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Entities.Models
{
    public class Rating
    {
        public int Id { get; set; }
        [Range(0, 10)]
        public int Rate { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime Created { get; set; }
    }
}
