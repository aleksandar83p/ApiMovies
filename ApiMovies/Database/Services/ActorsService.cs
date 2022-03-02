using ApiMovies.Database.Services.Interface;
using ApiMovies.Entities.DTO;
using ApiMovies.Entities.Models;
using ApiMovies.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiMovies.Database.Services
{
    public class ActorsService : IActorsService, IDisposable
    {
        private ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private readonly string _containerName = "Actors";

        public ActorsService(ApplicationDbContext context, IMapper mapper, IFileStorageService fileStorageService)
        {
            _context = context;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }
        public async Task<List<ActorDTO>> GetAllActorsAsync()
        {
            var actors = await _context.Actors.ToListAsync();
            var actorsDTO = _mapper.Map<List<ActorDTO>>(actors);

            return actorsDTO;
        }

        public async Task<ActorDTO> GetActorByIdAsync(int id)
        {
            var actor = await _context.Actors.FirstOrDefaultAsync(x => x.Id == id);
            var actorDto = _mapper.Map<ActorDTO>(actor);

            return actorDto;
        }

        public async Task AddActorAsync(ActorCreationDTO actorCreationDTO)
        {
            var actor = _mapper.Map<Actor>(actorCreationDTO);

            if (actorCreationDTO.Picture != null)
            {
                actor.Picture = await _fileStorageService.SaveFile(_containerName, actorCreationDTO.Picture);
            }

            actor.Created = DateTime.Now;

            await _context.Actors.AddAsync(actor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateActorAsync(int id, ActorUpdateDTO actorUpdateDTO)
        {
            var updateActor = await _context.Actors.FirstOrDefaultAsync(x => x.Id == id);

            if(updateActor != null)
            {
                _mapper.Map(actorUpdateDTO, updateActor);

                string updatePicture = "";
                if (actorUpdateDTO.Picture != null)
                {
                    updatePicture = await _fileStorageService.EditFile(_containerName, actorUpdateDTO.Picture, updateActor.Picture);
                }

                updateActor.Picture = updatePicture;

                await _context.SaveChangesAsync();
            }           
        }

        public async Task DeleteActorAsync(int id)
        {
            var deleteActor = await _context.Actors.FirstOrDefaultAsync(x => x.Id == id);

            if (deleteActor != null)
            {
                _context.Actors.Remove(deleteActor);
                await _fileStorageService.DeleteFile(deleteActor.Picture, _containerName);

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
