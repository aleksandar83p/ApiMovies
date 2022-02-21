using ApiMovies.Entities.DTO;
using ApiMovies.Entities.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiMovies.Database.Services;

namespace ApiMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService _GenreRepository;
        private readonly IMapper _Mapper;

        public GenresController(IGenresService genreRepository, IMapper mapper)
        {
            _GenreRepository = genreRepository;
            _Mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGenresAsync()
        {
            try
            {
                var genres = await _GenreRepository.GetAllAsync();
                var genresDTO = _Mapper.Map<List<GenreDTO>>(genres);
                return Ok(genresDTO);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetGenreByIdAsync(int id)
        {
            try
            {
                var genre = await _GenreRepository.GetByIdAsync(id);

                if (genre == null)
                {
                    return NotFound($"Genre with ID {id} not found.");
                }

                var genreDTO = _Mapper.Map<GenreDTO>(genre);
                return Ok(genreDTO);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostGenreAsync([FromBody] GenreCreationDTO genreCreationDTO)
        {
            try
            {
                var genre = _Mapper.Map<Genre>(genreCreationDTO);

                if (genreCreationDTO == null)
                {
                    return BadRequest();
                }

                genre.Created = DateTime.Now;
                await _GenreRepository.AddAsync(genre);
                return Created(nameof(PostGenreAsync), genre);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new genre record");
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutGenreAsync(int id, [FromBody] GenreDTO genreUpdateDTO)
        {
            try
            {
                var genre = await _GenreRepository.GetByIdAsync(id);

                if (id != genreUpdateDTO.Id)
                {
                    return BadRequest("Genre ID mismatch");
                }

                if (genreUpdateDTO == null)
                {
                    return NotFound($"Genre with ID = {id} not found.");
                }

                var updateGenre = _Mapper.Map<Genre>(genreUpdateDTO);
                updateGenre.Created = genre.Created;

                await _GenreRepository.UpdateAsync(id, updateGenre);
                return Ok(updateGenre);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating genre record");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteActorAsync(int id)
        {
            try
            {
                var deleteGenre = await _GenreRepository.GetByIdAsync(id);

                if (deleteGenre == null)
                {
                    return BadRequest($"Genre with ID = {id} not found.");
                }
           
                await _GenreRepository.DeleteAsync(id);

                return Ok($"Genre with ID {id} deleted.");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting genre record");
            }
        }
    }
}
