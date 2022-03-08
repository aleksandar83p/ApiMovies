using ApiMovies.Entities.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ApiMovies.Database.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ApiMovies.Controllers
{
    [Route("api/[controller]")]    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public class ActorsController : BaseController
    {
        private readonly IActorsService _service;        
        public ActorsController(IActorsService actorRepository)
        {
            _service = actorRepository;           
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync([FromQuery] string sortBy, [FromQuery] string searchString, [FromQuery] int? pageNumber)
        {
            var actors = await _service.GetAllActorsAsync(sortBy, searchString, pageNumber);
            return Ok(actors);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var actor = await _service.GetActorByIdAsync(id);

            if (actor == null)
            {
                return NotFound($"Actor with ID {id} not found.");
            }

            return Ok(actor);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromForm] ActorCreationDTO actorCreationDTO)
        {
            if (actorCreationDTO == null)
            {
                return BadRequest();
            }

            await _service.AddActorAsync(actorCreationDTO);
            return Created(nameof(PostAsync), actorCreationDTO);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(int id, [FromForm] ActorUpdateDTO actorUpdateDTO)
        {
            if (id != actorUpdateDTO.Id)
            {
                return BadRequest("Actor ID mismatch");
            }

            if (actorUpdateDTO == null)
            {
                return NotFound($"Actor with ID = {id} not found.");
            }

            await _service.UpdateActorAsync(id, actorUpdateDTO);
            return Ok(actorUpdateDTO);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var deleteActor = await _service.GetActorByIdAsync(id);

            if (deleteActor == null)
            {
                return NotFound($"Actor with ID = {id} not found.");
            }

            await _service.DeleteActorAsync(id);

            return Ok($"Actor with ID {id} deleted.");
        }
    }
}
