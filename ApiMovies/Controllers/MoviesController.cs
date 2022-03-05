using ApiMovies.Database.Services.Interface;
using ApiMovies.Entities.DTO;
using ApiMovies.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ApiMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _service;       
        private readonly IFileStorageService _fileStorageService;
        private readonly string _containerName = "Movies";

        public MoviesController(IMoviesService moviesService, IFileStorageService fileStorageService)
        {
            _service = moviesService;          
            _fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var movies = await _service.GetAllMoviesAsync();               
                return Ok(movies);
            }
            catch (Exception e)
            {
                return Content($"{e}", "application/json");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
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
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
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
            catch(Exception e)
            {
                return Content($"{e}", "application/json");  
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new movie record");
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
            catch (Exception e)
            {
                return Content($"{e}", "application/json");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating actor record");
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

                if(deleteMovie.Poster != null)
                {
                    await _fileStorageService.DeleteFile(deleteMovie.Poster, _containerName);
                }
                
                await _service.DeleteMoviesAsync(id);

                return Ok($"Movie with ID {id} deleted.");
            }
            catch (Exception e)
            {
                return Content($"{e}", "application/json");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting actor record");
            }            
        }
    }
}
