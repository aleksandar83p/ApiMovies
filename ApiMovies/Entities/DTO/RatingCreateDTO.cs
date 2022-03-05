using System;
using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Entities.DTO
{
    public class RatingCreateDTO
    {
        [Range(1,10)]
        public int Rating { get; set; }
        public int MovieID { get; set; }
    }
}
