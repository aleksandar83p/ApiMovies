using ApiMovies.Database.Services.Interface;
using ApiMovies.Entities.DTO;
using ApiMovies.Entities.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiMovies.Database.Services
{
    public class GenresService : IGenresService, IDisposable
    {
        private ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GenresService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GenreDTO>> GetAllGenresAsync()
        {
            var genres = await _context.Genres.ToListAsync();
            var genresDTO = _mapper.Map<List<GenreDTO>>(genres);

            return genresDTO;
        }

        public async Task<GenreDTO> GetGenreByIdAsync(int id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Id == id);
            var genreDto = _mapper.Map<GenreDTO>(genre);

            return genreDto;
        }

        public async Task AddGenreAsync(GenreCreationDTO genreCreationDTO)
        {
            var genre = _mapper.Map<Genre>(genreCreationDTO);
            genre.Created = DateTime.Now;

            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateGenreAsync(int id, GenreDTO genreUpdateDTO)
        {
            var updateGenre = await _context.Genres.FirstOrDefaultAsync(x => x.Id == id);

            if(updateGenre != null)
            {
                _mapper.Map(genreUpdateDTO, updateGenre);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteGenreAsync(int id)
        {
            var deleteGenre = await _context.Genres.FirstOrDefaultAsync(x => x.Id == id);

            if(deleteGenre != null)
            {
                _context.Genres.Remove(deleteGenre);
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
