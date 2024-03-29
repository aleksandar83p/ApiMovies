﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Entities.Models
{
    public class Movie
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
    }
}
