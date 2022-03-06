using ApiMovies.Database.Services.Interface;
using ApiMovies.Entities.DTO;
using ApiMovies.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ApiMovies.Database.Services
{
    public class RatingService : IRatingService, IDisposable
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;       

        public RatingService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task AddRatingAsync(string email, RatingCreateDTO ratingCreateDTO)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var userId = user.Id;

            var currentRate = await _context.Ratings
                .FirstOrDefaultAsync(x => x.MovieId == ratingCreateDTO.MovieID && x.UserId == userId);

            if(currentRate == null)
            {
                var rating = new Rating();
                rating.MovieId = ratingCreateDTO.MovieID;
                rating.Rate = ratingCreateDTO.Rating;
                rating.UserId = userId;
                rating.Created = DateTime.Now;
                _context.Add(rating);
            }
            else
            {
                currentRate.Rate = ratingCreateDTO.Rating;
            }

            await _context.SaveChangesAsync();  
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
