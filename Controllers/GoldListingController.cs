using ApiWgold.Context;
using ApiWgold.Models;
using ApiWow.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace ApiWgold.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class GoldListingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GoldListingController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("order/{goldListingId:int}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetGoldListingOrder(int goldListingId)
        {
            try
            {
                var orders = await _context.Order.Where(o => o.GoldListingId == goldListingId).ToListAsync();
                if(!orders.Any())
                {
                    return NotFound($"Nenhum pedido encontrado para o anuncio com id {goldListingId}");
                }
                return Ok(orders);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }


        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GoldListingDTO>>> Get()
        {
            try
            {
                var listings = await _context.GoldListing
                    .Include(g => g.Server)
                    .Include(g => g.User)
                    .Include(g => g.Server.Games)
                    .Select(g => new GoldListingDTO
                    {
                        GoldListingId = g.GoldListingId,
                        User = new UserSummaryDTO
                        {
                            UserId = g.User.UserId,
                            Username = g.User.Username
                        },
                        Server = new ServerDTO
                        {
                            ServerId = g.Server.ServerId,
                            ServerName = g.Server.ServerName,
                            Game = new GameDTO
                            {
                                GameId = g.Server.Games.GameId,
                                Name = g.Server.Games.Name
                            }
                        },
                        PricePerK = g.PricePerK,
                        Qtd = g.Qtd,
                        Faccao = g.Faccao,
                        CreatedAt = g.CreatedAt
                    })
                    .ToListAsync();

                return Ok(listings);
            }
            catch (Exception){
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }

        [HttpGet("{id:int}", Name = "ObterGoldListing")]
        public async Task<ActionResult<GoldListingDTO>> Get(int id)
        {
            try
            {
                var listings = await _context.GoldListing.AsNoTracking()
                    .Where(g => g.GoldListingId == id)
                    .Include(g => g.Server)
                    .Include(g => g.User)
                    .Include(g => g.Server.Games)
                    .Select(g => new GoldListingDTO
                    {
                        GoldListingId = g.GoldListingId,
                        User = new UserSummaryDTO
                        {
                            UserId = g.User.UserId,
                            Username = g.User.Username
                        },
                        Server = new ServerDTO
                        {
                            ServerId = g.Server.ServerId,
                            ServerName = g.Server.ServerName,
                            Game = new GameDTO
                            {
                                GameId = g.Server.Games.GameId,
                                Name = g.Server.Games.Name
                            }
                        },
                        PricePerK = g.PricePerK,
                        Qtd = g.Qtd,
                        Faccao = g.Faccao,
                        CreatedAt = g.CreatedAt
                    })
                    .FirstOrDefaultAsync();

                return Ok(listings);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }


        [HttpPost]
        public ActionResult<GoldListing> Post(GoldListing goldListing)
        {
            try
            {
                if(goldListing is null)
                {
                    return BadRequest("Anuncio não pode ser nulo");
                }

                _context.GoldListing.Add(goldListing);
                _context.SaveChanges();

                return new CreatedAtRouteResult("ObterGoldListing",
                new{id = goldListing.GoldListingId}, goldListing);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }

        [HttpPatch("{id:int}")]
        public ActionResult<GoldListing> Patch(int id, GoldListing goldListing)
        {
            try
            {
                if(id != goldListing.GoldListingId)
                {
                    return BadRequest("Dados Invalidos");
                }

                _context.Entry(goldListing).State = EntityState.Modified;
                _context.SaveChanges();
                return Ok(goldListing);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult<GoldListing> Delete(int id)
        {
            try
            {
                var goldListing = _context.GoldListing.FirstOrDefault(g => g.GoldListingId == id);
                if(goldListing == null)
                {
                    return NotFound($"Nenhum anuncio encontrado com o id {id}");
                }
                _context.GoldListing.Remove(goldListing);
                _context.SaveChanges();
                return Ok(goldListing);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }



    
    }

}