using ApiWgold.Context;
using ApiWgold.Models;
using ApiWow.DTOs;
using ApiWow.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiWow.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        
        public AuthController(ITokenService tokenService, 
                              AppDbContext context,
                              IConfiguration configuration)
        {
            _tokenService = tokenService;
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == model.Email);

            if(user is not null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                var claim = new[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
            }
        }
    };

    
}
