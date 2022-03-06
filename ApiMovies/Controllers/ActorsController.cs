using ApiMovies.Entities.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ApiMovies.Database.Services.Interface;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ApiMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public class ActorsController : ControllerBase
    {
        private readonly IActorsService _service;      

        public ActorsController(IActorsService actorRepository)
        {
            this._service = actorRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync([FromQuery] string sortBy, [FromQuery] string searchString, [FromQuery] int? pageNumber)
        {
            try
            {
                var actors = await _service.GetAllActorsAsync(sortBy, searchString, pageNumber);                
                return Ok(actors);
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
                var actor = await _service.GetActorByIdAsync(id);

                if(actor == null)
                {
                    return NotFound($"Actor with ID {id} not found.");
                }
                
                return Ok(actor);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromForm] ActorCreationDTO actorCreationDTO)
        {
            try
            {
                if (actorCreationDTO == null)
                {
                    return BadRequest();
                }
                             
                await _service.AddActorAsync(actorCreationDTO);                
                return Created(nameof(PostAsync), actorCreationDTO);
            }
            catch (Exception e)
            {
                return Content($"{e}", "application/json");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new actor record");
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(int id, [FromForm] ActorUpdateDTO actorUpdateDTO)
        {
            try
            {
               // var actor = await _service.GetActorByIdAsync(id);                

                if (id != actorUpdateDTO.Id)
                {
                    return BadRequest("Actor ID mismatch");
                }                

                if(actorUpdateDTO == null)
                {
                    return NotFound($"Actor with ID = {id} not found.");
                }            

                await _service.UpdateActorAsync(id, actorUpdateDTO);
                return Ok(actorUpdateDTO);
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

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var deleteActor = await _service.GetActorByIdAsync(id);

                if(deleteActor == null)
                {
                    return NotFound($"Actor with ID = {id} not found.");
                }
                
                await _service.DeleteActorAsync(id);

                return Ok($"Actor with ID {id} deleted.");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting actor record");
            }
        }
    }
}
