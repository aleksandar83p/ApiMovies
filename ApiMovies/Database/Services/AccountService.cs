using ApiMovies.Database.Services.Interface;
using ApiMovies.Entities.DTO;
using ApiMovies.Entities.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiMovies.Database.Services
{
    public class AccountService : IAccountService, IDisposable
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;        
        private readonly IConfiguration _configuration;
        private ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AccountService(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              IConfiguration configuration,
                              ApplicationDbContext context,
                              IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<AuthenticationResponse> RegisterAsync(UserCredentials userCredentials)
        {
            var user = new ApplicationUser
            {
                UserName = userCredentials.Email,
                Email = userCredentials.Email,
            };

            var result = await _userManager.CreateAsync(user, userCredentials.Password);

            if (result.Succeeded)
            {
                return await BuildTokenAsync(userCredentials);
            }

            return null;
        }

        public async Task<AuthenticationResponse> LoginAsync(UserCredentials userCredentials)
        {
            var result = await _signInManager.PasswordSignInAsync(userCredentials.Email,
                userCredentials.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return await BuildTokenAsync(userCredentials);
            }

            return null;
        }

        public async Task MakeAdminAsync(AdminDTO aminDTO)
        {
            var user = await _userManager.FindByIdAsync(aminDTO.UserId);
            if(user != null)
            {
                await _userManager.AddClaimAsync(user, new Claim("role", "admin"));
            }            
        }

        public async Task RemoveAdminAsync(AdminDTO adminDTO)
        {
            var user = await _userManager.FindByIdAsync(adminDTO.UserId);
            if (user != null)
            {
                await _userManager.RemoveClaimAsync(user, new Claim("role", "admin"));
            }            
        }

        public async Task<List<UserDTO>> GetListOfUsersAsync(string sortBy, string searchString, int? pageNumber)
        {
            var users = await _context.Users.OrderBy(x => x.Email).ToListAsync();


            // Sort By
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "name_desc":
                        users =  users.OrderByDescending(x => x.Email).ToList();
                        break;
                    default:
                        break;
                }
            }

            // Search String
            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(n => n.Email.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            // Pagination
            int pageSize = 5;
            users = PaginatedList<ApplicationUser>.Create(users.AsQueryable(), pageNumber ?? 1, pageSize);

            return _mapper.Map<List<UserDTO>>(users);
        }

        private async Task<AuthenticationResponse> BuildTokenAsync(UserCredentials userCredentials)
        {
            // Do not put a password or something else confidential.
            var claims = new List<Claim>();
            {
                new Claim("email", userCredentials.Email);
            };

            var user = await _userManager.FindByEmailAsync(userCredentials.Email);
            var claimDB = await _userManager.GetClaimsAsync(user);

            claims.AddRange(claimDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["keyjwt"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddDays(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiration, signingCredentials: creds);

            return new AuthenticationResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
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
