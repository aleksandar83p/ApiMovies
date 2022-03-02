using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMovies.Entities.DTO
{
    public class MovieUpdateDTO
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 75)]
        [Required]
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Trailer { get; set; }
        public DateTime ReleaseDate { get; set; }
        public IFormFile Poster { get; set; }
        public List<int> MovieGenres { get; set; }
        public List<int> MovieActors { get; set; }
    }
}
