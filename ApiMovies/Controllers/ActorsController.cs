using ApiMovies.Entities.DTO;
using ApiMovies.Entities.Models;
using ApiMovies.Helpers;
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
    public class ActorsController : ControllerBase
    {
        private readonly IActorsService _service;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private readonly string _containerName = "Actors";

        public ActorsController(IActorsService actorRepository, IMapper mapper, IFileStorageService fileStorageService)
        {
            this._service = actorRepository;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var actors = await _service.GetAllAsync();
                var actorsDTO = _mapper.Map<List<ActorDTO>>(actors);
                return Ok(actorsDTO);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var actor = await _service.GetByIdAsync(id);

                if(actor == null)
                {
                    return NotFound($"Actor with ID {id} not found.");
                }

                var actorDTO = _mapper.Map<ActorDTO>(actor);
                return Ok(actorDTO);
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
                var actor = _mapper.Map<Actor>(actorCreationDTO);

                if (actorCreationDTO == null)
                {
                    return BadRequest();
                }

                if (actorCreationDTO.Picture != null)
                {
                    actor.Picture = await _fileStorageService.SaveFile(_containerName, actorCreationDTO.Picture);
                }

                actor.Created = DateTime.Now;                
                await _service.AddAsync(actor);                
                return Created(nameof(PostAsync), actor);               
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
                var actor = await _service.GetByIdAsync(id);                

                if (id != actorUpdateDTO.Id)
                {
                    return BadRequest("Actor ID mismatch");
                }                

                if(actorUpdateDTO == null)
                {
                    return NotFound($"Actor with ID = {id} not found.");
                }

                string pictureDB = "";

                if(actorUpdateDTO.Picture != null)
                {
                    pictureDB = await _fileStorageService.EditFile(_containerName, actorUpdateDTO.Picture, actor.Picture);                   
                }

                var updateActor = _mapper.Map<Actor>(actorUpdateDTO);
                updateActor.Picture = pictureDB;
                updateActor.Created = actor.Created;

                await _service.UpdateAsync(id, updateActor);
                return Ok(updateActor);
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
                var deleteActor = await _service.GetByIdAsync(id);

                if(deleteActor == null)
                {
                    return NotFound($"Actor with ID = {id} not found.");
                }

                await _fileStorageService.DeleteFile(deleteActor.Picture, _containerName);
                await _service.DeleteAsync(id);

                return Ok($"Actor with ID {id} deleted.");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting actor record");
            }
        }
    }
}
