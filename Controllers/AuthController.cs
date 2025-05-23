using ApiWgold.Context;
using ApiWgold.Models;
using ApiWow.DTOs;
using ApiWow.Services;
using Microsoft.AspNetCore.Authorization;
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
                    //new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var token = _tokenService.GenerateAccesToken(claim, _configuration); 
                
                var refreshToken = _tokenService.GenerateRefreshToken();    

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);

                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(refreshTokenValidityInMinutes);

                user.RefreshToken = refreshToken;

                await _context.SaveChangesAsync();
                
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo,

                });
            }

            return Unauthorized(new
            {
                message = "Email ou senha invalidos"
            });
        }


        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] RegisterModel model)
        {
            var userExist = await _context.User.FirstOrDefaultAsync(u => u.Email == model.Email || u.Username == model.UserName);

            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error",
                        Message = "Usuario ja existe"
                    });
            }

            var newUser = new User()
            {
                Username = model.UserName,
                Email = model.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role = "UsuarioBase",
                CreatedAt = DateTime.UtcNow,
            };

            var result = await _context.User.AddAsync(newUser);
            var saved = await _context.SaveChangesAsync();

            if (saved <= 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = "Falha na criacao do usuario" });
            }
            return Ok(new Response { Status = "Success", Message = "Usuario criado com sucesso" });

        }


        //[Authorize]
        [HttpPost]
        [Route("register/adm")]
        public async Task<ActionResult> RegisterAdm([FromBody] RegisterModel model)
        {
            var userExist = await _context.User.FirstOrDefaultAsync(u => u.Email == model.Email || u.Username == model.UserName);

            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response
                    {
                        Status = "Error",
                        Message = "Usuario ja existe"
                    });
            }

            var newUser = new User()
            {
                Username = model.UserName,
                Email = model.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role = "Admin",
                CreatedAt = DateTime.UtcNow,
            };

            var result = await _context.User.AddAsync(newUser);
            var saved = await _context.SaveChangesAsync();

            if (saved <= 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = "Falha na criacao do usuario" });
            }
            return Ok(new Response { Status = "Success", Message = "Usuario criado com sucesso" });

        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<ActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("invalid client request");
            }

            string? accessToken = tokenModel.AccessToken
                ?? throw new ArgumentNullException(nameof(tokenModel));

            string? refreshToken = tokenModel.RefreshToken
                ?? throw new ArgumentNullException(nameof(tokenModel));

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken, _configuration);

            if(principal == null)
            {
                return BadRequest("token invalido");
            }

            string email = principal.FindFirst(ClaimTypes.Email)?.Value
                ?? principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value;

            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return BadRequest("token invalido");
            }

            var newAccessToken = _tokenService.GenerateAccesToken(principal.Claims.ToList(), _configuration);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            await _context.SaveChangesAsync();

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken,
            });
        }

        [Authorize]
        [HttpPost]
        [Route("revoke/{Username}")]
        public async Task<ActionResult> Revoke(String username)
        { 
            var user = await _context.User.FindAsync(username);
            if (user == null) {
                return BadRequest("Usuario nao encontrado");
            }

            user.RefreshToken = null;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [HttpGet]
        [Route("validate")]
        public async Task<ActionResult<UserDTO>> GetUserLogado()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized();
            }

            var user = await _context.User
                .Where(u => u.Email == userEmail)
                .Select(u => new UserDTO
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    Email = u.Email,
                    Role = u.Role,
                    ChaveVendedor = u.ChaveVendedor,
                    CreatedAt = u.CreatedAt
                }).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("Usuario nao encontrado");
            }

            return Ok(user);
        }

    };

    
}
