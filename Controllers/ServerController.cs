using ApiWgold.Context;
using ApiWgold.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ApiGold.Controllers
{
    [Route("[Controller]")]
    [ApiController]

    public class ServerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("goldlisting/{serverId:int}")]
        public ActionResult<IEnumerable<GoldListing>> GetServerGoldListing(int serverId)
        {
            try
            {
                var goldListings = _context.GoldListing.Where(gl => gl.ServerId == serverId).ToList();

                if(!goldListings.Any())
                {
                    return NotFound($"Nenhum anuncio encontrado para o servidor com id {serverId}");
                }
                return Ok(goldListings);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Server>> Get()
        {
            try
            {
                return _context.Server.ToList();            
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }

        [HttpGet("{id:int}", Name = "ObterServer")]
        public ActionResult<Server> Get(int id)
        {
            try
            {
                var server = _context.Server.FirstOrDefault(s => s.ServerId == id);
                if (server == null)
                {
                    return NotFound($"Servidor com id {id} não encontrado");
                }
                return Ok(server);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }

        [HttpPost]
        public ActionResult Post(Server server)
        {
            try
            {
                if(server == null)
                {
                    return BadRequest("Servidor não pode ser nulo");
                }
                _context.Server.Add(server);
                _context.SaveChanges();

                return new CreatedAtRouteResult("ObterServer",
                new{id = server.ServerId}, server);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }
    }
}