using ApiWgold.Context;
using ApiWgold.Models;
using ApiWow.DTOs;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
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
        [Authorize]
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
        //[Authorize]
        public async Task<ActionResult<IEnumerable<UserDTO>>> Get()
        {
            try
            {
                // Ensure the query is not null and map User to UserDTO
                var users = await _context.User
                    .AsNoTracking()
                    .Select(u => new UserDTO
                    {
                        UserId = u.UserId,
                        Username = u.Username,
                        Email = u.Email,
                        Role = u.Role,
                        ChaveVendedor = u.ChaveVendedor,
                        CreatedAt = u.CreatedAt
                    })
                    .ToListAsync();

                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }

        [HttpGet("{id:int}", Name = "ObterUsuario")]
        [Authorize]
        public async Task<ActionResult<User>> Get(int id)
        {

            //throw new Exception("excecao ao retornao usuario pelo id");
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



        [HttpPatch("{id:int}")]
        [Authorize]
        public ActionResult Aprovar(int id)
        {
            try
            {
                var user = _context.User.FirstOrDefault(u => u.UserId == id);
                if(user == null)
                {
                    return BadRequest("Dados invalidos");
                }

                user.Role = "UserVendedor";


                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();

                var result = new UserDTO()
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                };
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }


       

        [HttpDelete("{id:int}")]
        [Authorize]
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