using ApiMovies.Database.Services.Interface;
using ApiMovies.Entities.DTO;
using ApiMovies.Entities.Models;
using ApiMovies.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        public async Task<List<MovieDTO>> GetAllMoviesAsync()
        {
            var actorsForThisMovie = new List<Actor>();
            var genresForThisMovie = new List<Genre>();

            var dtoMovies = new List<MovieDTO>();

            var movies = await _context.Movie.ToListAsync();
            var actors = await _context.Actors.ToListAsync();
            var movieActors = await _context.MovieActors.ToListAsync();
            var genres = await _context.Genres.ToListAsync();
            var movieGenrs = await _context.MovieGenres.ToListAsync();

            foreach (var movie in movies)
            {
                MovieDTO movieDto = new MovieDTO();
                movieDto.Movie_Actors = new List<ActorDTO>();
                movieDto.Movie_Genres = new List<GenreDTO>();
                movieDto.Id = movie.Id;
                movieDto.Title = movie.Title;
                movieDto.Trailer = movie.Trailer;
                movieDto.Summary = movie.Summary;
                movieDto.ReleaseDate = movie.ReleaseDate;
                movieDto.Created = DateTime.Now;   

                // load actors for movie
                var movieActorsForThisMovie = movieActors.Where(x => x.MovieId == movie.Id);
                if (movieActorsForThisMovie.Any())
                {
                    foreach (var mActor in movieActorsForThisMovie)
                    {
                        var actor = actors.Where(x => x.Id == mActor.ActorId).FirstOrDefault();
                        var actorDto = new ActorDTO();

                        actorDto.Id = actor.Id;
                        actorDto.Name = actor.Name;
                        actorDto.Biography = actor.Biography;
                        actorDto.DateOfBirth = actor.DateOfBirth;
                        actorDto.Picture = actor.Picture;
                        
                        movieDto.Movie_Actors.Add(actorDto);                       
                    }
                }

                // load genres for movie
                var movieGenresForThisMovie = movieGenrs.Where(x => x.MovieId == movie.Id);
                foreach (var mGenre in movieGenresForThisMovie)
                {
                    var genre = genres.Where(x => x.Id == mGenre.GenreId).FirstOrDefault();

                    var genreDto = new GenreDTO();
                    genreDto.Id = genre.Id;
                    genreDto.Name = genre.Name;

                    movieDto.Movie_Genres.Add(genreDto);
                }
                dtoMovies.Add(movieDto);
            }
            return dtoMovies;
        }

        public async Task<MovieDTO> GetMoviesByIdAsync(int id)
        {
            var dtoMovie = new MovieDTO();
            var actorsForThisMovie = new List<Actor>();
            var genresForThisMovie = new List<Genre>();            

            var movie = await _context.Movie.FirstOrDefaultAsync(x => x.Id == id);

            if(movie == null)
            {
                return null;
            }

            var actors = await _context.Actors.ToListAsync();
            var movieActors = await _context.MovieActors.ToListAsync();
            var genres = await _context.Genres.ToListAsync();
            var movieGenrs = await _context.MovieGenres.ToListAsync();

            dtoMovie.Movie_Actors = new List<ActorDTO>();
            dtoMovie.Movie_Genres = new List<GenreDTO>();
            dtoMovie.Id = movie.Id;
            dtoMovie.Title = movie.Title;
            dtoMovie.Trailer = movie.Trailer;
            dtoMovie.Summary = movie.Summary;
            dtoMovie.ReleaseDate = movie.ReleaseDate;
            dtoMovie.Created = DateTime.Now;

            // load actors for movie
            var movieActorsForThisMovie = movieActors.Where(x => x.MovieId == movie.Id);

                if (movieActorsForThisMovie.Any())
                {
                    foreach (var mActor in movieActorsForThisMovie)
                    {
                        var actor = actors.Where(x => x.Id == mActor.ActorId).FirstOrDefault();
                        var actorDto = new ActorDTO();
                        actorDto.Id = actor.Id;
                        actorDto.Name = actor.Name;
                        actorDto.Biography = actor.Biography;
                        actorDto.DateOfBirth = actor.DateOfBirth;
                        actorDto.Picture = actor.Picture;
                        dtoMovie.Movie_Actors.Add(actorDto);
                    }
                }

                // load genres for movie
                var movieGenresForThisMovie = movieGenrs.Where(x => x.MovieId == movie.Id);
                foreach (var mGenre in movieGenresForThisMovie)
                {
                    var genre = genres.Where(x => x.Id == mGenre.GenreId).FirstOrDefault();

                    var genreDto = new GenreDTO();
                    genreDto.Id = genre.Id;
                    genreDto.Name = genre.Name;

                dtoMovie.Movie_Genres.Add(genreDto);
                }               
            return dtoMovie;
        }

        public async Task AddMoviesAsync(MovieCreationDTO movieCreationDTO)
        {
            var movie = _mapper.Map<Movie>(movieCreationDTO);  

            if(movieCreationDTO.Poster != null)
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

            //var movie = new Movie();
            //movie.Id = movieDtoUpdate.Id; 
            //movie.ReleaseDate = movieDtoUpdate.ReleaseDate;
            //movie.Summary = movieDtoUpdate.Summary;
            //movie.Title = movieDtoUpdate.Title;
            //movie.Trailer = movieDtoUpdate.Trailer;

            //EntityEntry updateMovie = _context.Entry<Movie>(movie);
            //updateMovie.State = EntityState.Modified;

                      
            if(movieDtoUpdate.MovieGenres != null)
            {
                var movieGenreList = _context.MovieActors.Where(x => x.MovieId == id).ToListAsync();

                for (int i = 0; i < movieDtoUpdate.MovieGenres.Count; i++)
                {
                    var movieGenre = new Movie_Genre();
                    movieGenre.MovieId = id;
                    movieGenre.GenreId = movieDtoUpdate.MovieGenres[i];

                    //movieGenreList.Add(movieGenre);

                    //_context.Entry(movieGenre).State = EntityState.Modified;
                    //await _context.SaveChangesAsync();
                }

                //foreach (var item in movieGenreList)
                //{
                //    await _context.SaveChangesAsync();
                //}
            }



            //foreach (var item in movieDtoUpdate.MovieGenres)
            //{

            //    genre.MovieId = movieDtoUpdate.Id;
            //    genre.GenreId = genreId;
            //    genre.Created = DateTime.Now;
            //    //EntityEntry updateGenre = _context.Entry<Movie_Genre>(genre);
            //    //updateGenre.State = EntityState.Modified;
            //    await _context.MovieGenres.AddAsync(genre);
            //    await _context.SaveChangesAsync();                
            //}

            
            if (movieDtoUpdate.MovieActors != null)
            {
                for (int i = 0; i < movieDtoUpdate.MovieActors.Count; i++)
                {
                    var movieActor = new Movie_Actor();
                    movieActor.MovieId = id;
                    movieActor.ActorId = movieDtoUpdate.MovieGenres[i];                    

                    await _context.SaveChangesAsync();
                }
            }


            //var actor = new Movie_Actor();
            //foreach (var actorId in movieDtoUpdate.MovieActors)
            //{
            //    actor.MovieId = movieDtoUpdate.Id;
            //    actor.ActorId = actorId;
            //    actor.Created = DateTime.Now;
            //    //EntityEntry updateActor = _context.Entry<Movie_Actor>(actor);
            //    //updateActor.State = EntityState.Modified;
            //    await _context.MovieActors.AddAsync(actor);
            //    await _context.SaveChangesAsync();
            //}            
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
    }
}
