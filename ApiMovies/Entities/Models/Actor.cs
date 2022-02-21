using ApiMovies.Database.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Entities.Models
{
    public class Actor : IEntityBase
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Biography { get; set; }
        public string Picture { get; set; }
        public DateTime Created { get; set; }

        public List<Movie_Actor> Actor_Movies{ get; set; }
    }
}
