using ApiWgold.Context;
using ApiWgold.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiWgold.Controllers
{
    [Route("[Controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("goldlisting/{userId:int}")]
        public async Task<ActionResult<IEnumerable<GoldListing>>> GetUserGoldListing(int userId)
        {
            try
            {
                var goldListings = await _context.GoldListing.Where(gl => gl.UserId == userId).ToListAsync();

                if(!goldListings.Any())
                {
                    return NotFound($"Nenhum anuncio encontrado para o usuario com id {userId}");
                }

                return Ok(goldListings);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }

        [HttpGet("order/{userId:int}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetUserOrder(int userId)
        {
            try
            {
                var orders = await _context.Order.Where(o => o.BuyerId == userId).ToListAsync();
                if(!orders.Any())
                {
                    return NotFound($"Nenhum pedido encontrado para o usuario com id {userId}");
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
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            try
            {
                return await _context.User.AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }

        [HttpGet("{id:int}", Name = "ObterUsuario")]
        public async Task<ActionResult<User>> Get(int id)
        {
            try
            {
                var user = await _context.User.FirstOrDefaultAsync(u => u.UserId == id);
                if(user == null)
                {
                    return NotFound($"Usuario com id {id} não encontrado");
                }
                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }

        [HttpPost]
        public ActionResult Post(User user)
        {
            try
            {
                if(user is null)
                {
                    return BadRequest("Dados Invalidos");
                }

                    _context.User.Add(user);
                    _context.SaveChanges();

                    return new CreatedAtRouteResult("ObterUsuario",
                    new{id = user.UserId}, user);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }


        [HttpPatch("{id:int}")]
        public ActionResult Patch(int id, User user)
        {
            try
            {
                if(id != user.UserId)
                {
                    return BadRequest("Dados invalidos");
                }
                
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
                return Ok(user);
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
                var user = _context.User.FirstOrDefault(u => u.UserId == id);
                if(user == null)
                {
                    return NotFound($"Usuario com id {id} não encontrado");
                }
                _context.User.Remove(user);
                _context.SaveChanges();
                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }



        

        


    }
}