using ApiWgold.Context;
using ApiWgold.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public ActionResult<IEnumerable<Order>> GetGoldListingOrder(int goldListingId)
        {
            try
            {
                var orders = _context.Order.Where(o => o.GoldListingId == goldListingId).ToList();
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
        public ActionResult<IEnumerable<GoldListing>> Get()
        {
            try
            {
                return _context.GoldListing.ToList();
            }
            catch (Exception){
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }

        [HttpGet("{id:int}", Name = "ObterGoldListing")]
        public ActionResult<GoldListing> Get(int id)
        {
            try
            {
                var goldListings = _context.GoldListing.FirstOrDefault(g => g.GoldListingId == id);
                if (goldListings == null)
                {
                    return NotFound($"Nenhum anuncio encontrado com o id {id}");
                }
                return Ok(goldListings);
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