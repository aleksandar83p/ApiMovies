using ApiMovies.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMovies.Database
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie_Actor>().HasKey(x => new
            { 
                x.ActorId,
                x.MovieId
            });            

            modelBuilder.Entity<Movie_Genre>().HasKey(x => new
            {
                x.MovieId,
                x.GenreId                
            });           
        }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie_Actor> MovieActors { get; set; }
        public DbSet<Movie_Genre> MovieGenres { get; set; }
        public DbSet<Movie> Movie { get; set; }
    }
}
