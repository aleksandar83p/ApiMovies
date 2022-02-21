using ApiMovies.Database.Base;
using ApiMovies.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMovies.Database.Services
{
    public class GenresService : EntityBaseRepository<Genre>, IGenresService
    {
        public GenresService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
