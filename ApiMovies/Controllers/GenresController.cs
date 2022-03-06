using ApiMovies.Entities.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ApiMovies.Database.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ApiMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService _service;       

        public GenresController(IGenresService genreRepository)
        {
            _service = genreRepository;         
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllGenresAsync([FromQuery] string sortBy, [FromQuery] string searchString, [FromQuery] int? pageNumber)
        {
            try
            {
                var genres = await _service.GetAllGenresAsync(sortBy, searchString, pageNumber);                
                return Ok(genres);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGenreByIdAsync(int id)
        {
            try
            {
                var genre = await _service.GetGenreByIdAsync(id);

                if (genre == null)
                {
                    return NotFound($"Genre with ID {id} not found.");
                }
                
                return Ok(genre);
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
                if (genreCreationDTO == null)
                {
                    return BadRequest();
                }
             
                await _service.AddGenreAsync(genreCreationDTO);
                return Created(nameof(PostGenreAsync), genreCreationDTO);
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
                if (id != genreUpdateDTO.Id)
                {
                    return BadRequest("Genre ID mismatch");
                }

                if (genreUpdateDTO == null)
                {
                    return NotFound($"Genre with ID = {id} not found.");
                }              

                await _service.UpdateGenreAsync(id, genreUpdateDTO);
                return Ok(genreUpdateDTO);
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
                var deleteGenre = await _service.GetGenreByIdAsync(id);

                if (deleteGenre == null)
                {
                    return BadRequest($"Genre with ID = {id} not found.");
                }
           
                await _service.DeleteGenreAsync(id);

                return Ok($"Genre with ID {id} deleted.");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting genre record");
            }
        }
    }
}
