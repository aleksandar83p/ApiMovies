using ApiMovies.Database.Services.Interface;
using ApiMovies.Entities.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _service;        

        public MoviesController(IMoviesService moviesService)
        {
            _service = moviesService;            
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync([FromQuery] string sortBy, [FromQuery] string searchString, [FromQuery] int? pageNumber)
        {
            var movies = await _service.GetAllMoviesAsync(sortBy, searchString, pageNumber);
            return Ok(movies);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await _service.GetMoviesByIdAsync(id);

            if (movie == null)
            {
                return NotFound($"Movie with ID = {id} not found.");
            }

            return Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromForm] MovieCreationDTO movieCreationDTO)
        {
            if (movieCreationDTO == null)
            {
                return BadRequest();
            }

            await _service.AddMoviesAsync(movieCreationDTO);
            return Created(nameof(PostAsync), movieCreationDTO);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(int id, [FromForm] MovieUpdateDTO MovieDTOupdate)
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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var deleteMovie = await _service.GetMoviesByIdAsync(id);

            if (deleteMovie == null)
            {
                return NotFound($"Movie with ID = {id} not found.");
            }

            await _service.DeleteMoviesAsync(id);

            return Ok($"Movie with ID {id} deleted.");
        }
    }
}
