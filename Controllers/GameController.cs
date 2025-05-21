

using ApiWgold.Context;
using ApiWgold.Models;
using ApiWow.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiWgold.Controllers
{
    [Route("[Controller]")]
    [ApiController]

    public class GameController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GameController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("server/{gameId:int}")]
        public async Task<ActionResult<IEnumerable<Server>>> GetGameServer(int gameId)
        {
            try
            {
                var servers =await _context.Server.Where(s => s.GamesId == gameId).ToListAsync();
                if(!servers.Any())
                {
                    return NotFound($"Nenhum servidor encontrado para o game com id {gameId}");
                }
                return Ok(servers);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDTO>>> Get()
        {
            try
            {
                var games = await _context.Game.AsNoTracking().Select(g => new GameDTO
                {
                    GameId = g.GameId,
                    Name = g.Name,
                    CreatedAt = g.CreatedAt
                }).ToListAsync();

                return Ok(games);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }

        [HttpGet("{id:int}", Name = "ObterGame")]
        public async Task<ActionResult<Game>> Get(int id)
        {
            try
            {
                var game = await _context.Game.AsNoTracking().FirstOrDefaultAsync(g => g.GameId == id);
                if (game == null)
                {
                    return NotFound($"Game com id {id} não encontrado");
                }
                return Ok(game);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }

        [HttpPost]
        public ActionResult Post(Game game)
        {
            try
            {
                if(game is null)
                {
                    return BadRequest("Dados Invalidos");
                }

                _context.Game.Add(game);
                _context.SaveChanges();
                return new CreatedAtRouteResult("ObterGame",
                new{id = game.GameId}, game);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }

        [HttpPatch("{id:int}")]
        public ActionResult Patch(int id, Game game)
        {
            try
            {
                if (id != game.GameId)
                {
                    return BadRequest("Dados Invalidos");
                }

                _context.Entry(game).State = EntityState.Modified;
                _context.SaveChanges();
                return Ok(game);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var game = _context.Game.FirstOrDefault(g => g.GameId == id);
                if(game == null)
                {
                    return NotFound($"Game com id {id} não encontrado");
                }

                _context.Game.Remove(game);
                _context.SaveChanges();
                return Ok($"Game com id {id} deletado com sucesso");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }

    }
}