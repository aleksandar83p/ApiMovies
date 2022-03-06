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
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;        

        public AccountController(IAccountService service)
        {
            _service = service;            
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCredentials userCredentials)
        {
            var result = await _service.RegisterAsync(userCredentials);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result.Token);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserCredentials userCredentials)
        {
            var result = await _service.LoginAsync(userCredentials);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result.Token);
        }

        [HttpPost("makeAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
        public async Task<IActionResult> MakeAdmin([FromBody] AdminDTO makeAdminDTO)
        {
            await _service.MakeAdminAsync(makeAdminDTO);
            return NoContent();
        }

        [HttpPost("removeAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
        public async Task<IActionResult> RemoveAdmin([FromBody] AdminDTO makeAdminDTO)
        {
            await _service.RemoveAdminAsync(makeAdminDTO);
            return NoContent();
        }

        [HttpGet("listUsers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
        public async Task<IActionResult> GetListUsersAsync([FromQuery] string sortBy, [FromQuery] string searchString, [FromQuery] int? pageNumber)
        {
            var result = await _service.GetListOfUsersAsync(sortBy, searchString, pageNumber);
            return Ok(result);
        }
    }
}
