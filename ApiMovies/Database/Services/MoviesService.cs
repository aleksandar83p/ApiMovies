using ApiMovies.Database.Services.Interface;
using ApiMovies.Entities.DTO;
using ApiMovies.Entities.Models;
using ApiMovies.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMovies.Database.Services
{
    public class MoviesService : IMoviesService, IDisposable
    {
        private ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private readonly string _containerName = "Movies";

        public MoviesService(ApplicationDbContext context, IMapper mapper, IFileStorageService fileStorageService)
        {
            _context = context;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        public async Task<List<MovieDTO>> GetAllMoviesAsync(string sortBy, string searchString, int? pageNumber)
        {
            var dtoMovies = new List<MovieDTO>();
            var movies = await _context.Movie.ToListAsync();          

            foreach (var movie in movies)
            {
                var movieDto = _mapper.Map<MovieDTO>(movie);
                movieDto.Movie_Actors = await loadActorsForMovie(movie);
                movieDto.Movie_Genres = await loadGenresForMovie(movie);

                dtoMovies.Add(movieDto);
            }

            // Sorting
            dtoMovies = dtoMovies.OrderBy(x => x.Title).ToList();

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "title_desc":
                        dtoMovies = dtoMovies.OrderByDescending(x => x.Title).ToList();
                        break;
                    case "release_asc":
                        dtoMovies = dtoMovies.OrderBy(x => x.ReleaseDate).ToList();
                        break;
                    case "release_desc":
                        dtoMovies = dtoMovies.OrderByDescending(x => x.ReleaseDate).ToList();
                        break;
                    default:
                        break;
                }
            }

            // Search
            if (!string.IsNullOrEmpty(searchString))
            {
                dtoMovies = dtoMovies.Where(x => x.Title.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            // Pagination
            int pageSize = 5;
            dtoMovies = PaginatedList<MovieDTO>.Create(dtoMovies.AsQueryable(), pageNumber ?? 1, pageSize);

            return dtoMovies;
        }

        public async Task<MovieDTO> GetMoviesByIdAsync(int id)
        {
            var movie = await _context.Movie.FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null)
            {
                return null;
            }

            var dtoMovie = _mapper.Map<MovieDTO>(movie);
            dtoMovie.Movie_Actors = await loadActorsForMovie(movie);
            dtoMovie.Movie_Genres = await loadGenresForMovie(movie);            

            // load ratings for movie
            var averageVote = 0.0;
            // var userVote = 0;

            if (await _context.Ratings.AnyAsync(x => x.MovieId == id))
            {
                averageVote = await _context.Ratings.Where(x => x.MovieId == id).AverageAsync(x => x.Rate);
            }

            return dtoMovie;
        }

        public async Task AddMoviesAsync(MovieCreationDTO movieCreationDTO)
        {
            var movie = _mapper.Map<Movie>(movieCreationDTO);

            if (movieCreationDTO.Poster != null)
            {
                movie.Poster = await _fileStorageService.SaveFile(_containerName, movieCreationDTO.Poster);
            }

            movie.Created = DateTime.Now;
            await _context.Movie.AddAsync(movie);
            await _context.SaveChangesAsync();

            // add genres for movie        
            var ganres = await _context.Genres.ToListAsync();

            for (int i = 0; i < movieCreationDTO.MovieGenres.Count; i++)
            {
                var movieGenre = new Movie_Genre();
                movieGenre.MovieId = movie.Id;
                movieGenre.Genre = ganres.FirstOrDefault(x => x.Id == movieCreationDTO.MovieGenres[i]);
                movieGenre.Created = DateTime.Now;

                await _context.MovieGenres.AddAsync(movieGenre);
                await _context.SaveChangesAsync();
            }

            // add actors for movie
            var actors = await _context.Actors.ToListAsync();

            for (int i = 0; i < movieCreationDTO.MovieActors.Count; i++)
            {
                var movieActor = new Movie_Actor();
                movieActor.MovieId = movie.Id;
                movieActor.Actor = actors.First(x => x.Id == movieCreationDTO.MovieActors[i]);
                movieActor.Created = DateTime.Now;

                await _context.MovieActors.AddAsync(movieActor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateMoviesAsync(int id, MovieUpdateDTO movieDtoUpdate)
        {
            var updateMovie = await _context.Movie.FirstOrDefaultAsync(x => x.Id == id);

            if (updateMovie != null)
            {
                _mapper.Map(movieDtoUpdate, updateMovie);

                string updatePoster = "";
                if (movieDtoUpdate.Poster != null)
                {
                    updatePoster = await _fileStorageService.EditFile(_containerName, movieDtoUpdate.Poster, updateMovie.Poster);
                }

                updateMovie.Poster = updatePoster;

                await _context.SaveChangesAsync();
            }

            // ** TODO update genres and actors **
            
        }

        public async Task DeleteMoviesAsync(int movieId)
        {
            var deleteMovie = await _context.Movie.FirstOrDefaultAsync(x => x.Id == movieId);

            if (deleteMovie != null)
            {
                _context.Movie.Remove(deleteMovie);
                await _fileStorageService.DeleteFile(deleteMovie.Poster, _containerName);

                await _context.SaveChangesAsync();
            }
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private async Task<List<ActorDTO>> loadActorsForMovie(Movie movie)
        {
            var actorsList = new List<ActorDTO>();

            var actors = await _context.Actors.ToListAsync();
            var movieActors = await _context.MovieActors.ToListAsync();

            var movieActorsForThisMovie = movieActors.Where(x => x.MovieId == movie.Id);
            if (movieActorsForThisMovie.Any())
            {
                foreach (var movieActor in movieActorsForThisMovie)
                {
                    var actor = actors.Where(x => x.Id == movieActor.ActorId).FirstOrDefault();
                    var actorDto = _mapper.Map<ActorDTO>(actor);

                    actorsList.Add(actorDto);
                }
            }

            return actorsList;
        }

        private async Task<List<GenreDTO>> loadGenresForMovie(Movie movie)
        {
            var genreList = new List<GenreDTO>();

            var genres = await _context.Genres.ToListAsync();
            var movieGenrs = await _context.MovieGenres.ToListAsync();

            var movieGenresForThisMovie = movieGenrs.Where(x => x.MovieId == movie.Id);
            foreach (var movieGenre in movieGenresForThisMovie)
            {
                var genre = genres.Where(x => x.Id == movieGenre.GenreId).FirstOrDefault();
                var genreDto = _mapper.Map<GenreDTO>(genre);

                genreList.Add(genreDto);
            }

            return genreList;
        }
    }
}
