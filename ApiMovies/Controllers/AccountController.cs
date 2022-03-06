using ApiMovies.Database.Services.Interface;
using ApiMovies.Entities.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ApiMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService service, ILogger<AccountController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCredentials userCredentials)
        {
            try
            {
                var result = await _service.RegisterAsync(userCredentials);

                if (result != null)
                {
                    return Ok(result.Token);
                }
                else
                {
                    return BadRequest();
                }
            }            
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserCredentials userCredentials)
        {
            try
            {
                var result = await _service.LoginAsync(userCredentials);

                if (result != null)
                {
                    return Ok(result.Token);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }            
        }

        [HttpPost("makeAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
        public async Task<IActionResult> MakeAdmin([FromBody] AdminDTO makeAdminDTO)
        {
            try
            {
                await _service.MakeAdminAsync(makeAdminDTO);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }            
        }

        [HttpPost("removeAdmin")]
         [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
        public async Task<IActionResult> RemoveAdmin([FromBody] AdminDTO makeAdminDTO)
        {
            try
            {
                await _service.RemoveAdminAsync(makeAdminDTO);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }            
        }

        [HttpGet("listUsers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
        public async Task<IActionResult> GetListUsersAsync([FromQuery] string sortBy, [FromQuery] string searchString, [FromQuery] int? pageNumber)
        {
            try
            {
                var result = await _service.GetListOfUsersAsync(sortBy, searchString, pageNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }            
        }
    }
}
