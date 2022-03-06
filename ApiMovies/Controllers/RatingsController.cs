using ApiMovies.Database.Services.Interface;
using ApiMovies.Entities.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService _service;
        private readonly ILogger<RatingsController> _logger;

        public RatingsController(IRatingService service, ILogger<RatingsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostRatingAsync([FromBody] RatingCreateDTO ratingCreateDTO)
        {
            try
            {
                var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value;

                await _service.AddRatingAsync(email, ratingCreateDTO);
                return Created(nameof(PostRatingAsync), ratingCreateDTO.Rating);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
