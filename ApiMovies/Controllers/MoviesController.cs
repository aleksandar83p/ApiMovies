using ApiMovies.Database.Services.Interface;
using ApiMovies.Entities.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ApiMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _service;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(IMoviesService moviesService, ILogger<MoviesController> logger)
        {
            _service = moviesService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync([FromQuery] string sortBy, [FromQuery] string searchString, [FromQuery] int? pageNumber)
        {
            try
            {
                var movies = await _service.GetAllMoviesAsync(sortBy, searchString, pageNumber);               
                return Ok(movies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var movie = await _service.GetMoviesByIdAsync(id);
                if(movie == null)
                {
                    return NotFound($"Movie with ID = {id} not found.");
                }
                return Ok(movie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpPost]        
        public async Task<IActionResult> PostAsync([FromForm] MovieCreationDTO movieCreationDTO)
        {
            try
            {
                if (movieCreationDTO == null)
                {
                    return BadRequest();
                }
               
                await _service.AddMoviesAsync(movieCreationDTO);               
                return Created(nameof(PostAsync), movieCreationDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(int id, [FromForm] MovieUpdateDTO MovieDTOupdate)
        {
            try
            {
                if (id != MovieDTOupdate.Id)
                {
                    return BadRequest("Movie ID mismatch");
                }

                if (MovieDTOupdate == null)
                {
                    return NotFound($"Movie with ID = {id} not found.");
                }

                await _service.UpdateMoviesAsync(id, MovieDTOupdate);
                return Ok(MovieDTOupdate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var deleteMovie = await _service.GetMoviesByIdAsync(id);

                if (deleteMovie == null)
                {
                    return NotFound($"Movie with ID = {id} not found.");
                }
                
                await _service.DeleteMoviesAsync(id);

                return Ok($"Movie with ID {id} deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
