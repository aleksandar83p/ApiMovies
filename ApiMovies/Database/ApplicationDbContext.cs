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

            modelBuilder.Entity<Movie_Actor>().HasOne(m => m.Movie).WithMany(ma => ma.Movie_Actors).HasForeignKey(f => f.MovieId);
            modelBuilder.Entity<Movie_Actor>().HasOne(a => a.Actor).WithMany(am => am.Actor_Movies).HasForeignKey(f => f.ActorId);

            modelBuilder.Entity<Movie_Genre>().HasKey(x => new
            {
                x.GenreId,
                x.MovieId
            });

            modelBuilder.Entity<Movie_Genre>().HasOne(m => m.Movie).WithMany(mg => mg.Movie_Genres).HasForeignKey(f => f.GenreId);
            modelBuilder.Entity<Movie_Genre>().HasOne(g => g.Genre).WithMany(gm => gm.Genre_Movies).HasForeignKey(f => f.MovieId);
        }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie_Actor> MovieActors { get; set; }
        public DbSet<Movie_Genre> MovieGenres { get; set; }
    }
}
