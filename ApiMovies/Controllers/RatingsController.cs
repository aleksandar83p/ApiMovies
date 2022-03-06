using ApiMovies.Database.Services.Interface;
using ApiMovies.Entities.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService _service;     

        public RatingsController(IRatingService service)
        {
            _service = service;            
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostRatingAsync([FromBody] RatingCreateDTO ratingCreateDTO)
        {
            var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value;

            await _service.AddRatingAsync(email, ratingCreateDTO);
            return Created(nameof(PostRatingAsync), ratingCreateDTO.Rating);
        }
    }
}
