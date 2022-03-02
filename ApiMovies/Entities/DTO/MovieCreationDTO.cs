using ApiMovies.Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Entities.DTO
{
    public class MovieCreationDTO
    {
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
