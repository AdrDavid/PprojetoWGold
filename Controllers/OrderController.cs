using ApiWgold.Context;
using ApiWgold.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ApiGold.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Order>> Get()
        {
            try
            {
                return _context.Order.ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }


        [HttpGet("{id:int}", Name = "ObterOrder")]
        public ActionResult<Order> Get(int id)
        {
            try
            {
                var order = _context.Order.FirstOrDefault(o => o.OrderId == id);
                if(order == null)
                {
                    return NotFound($"Pedido com id {id} não encontrado");
                }
                return Ok(order);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }


        [HttpPost]
        public ActionResult Post(Order order)
        {
            try
            {
                if(order == null)
                {
                    return BadRequest("Pedido não pode ser nulo");
                }

                var goldListing = _context.GoldListing.FirstOrDefault(gl => gl.GoldListingId == order.GoldListingId);
                if(goldListing == null)
                {
                    return NotFound($"Anuncio com id {order.GoldListingId} não encontrado");
                }

                if(goldListing.Qtd < order.QuantityK)
                {
                    return BadRequest($"Quantidade de {goldListing.Qtd} é menor que a quantidade do pedido {order.QuantityK}");
                }


                goldListing.Qtd -= order.QuantityK;

                _context.Order.Add(order);
                _context.SaveChanges();

                return new CreatedAtRouteResult("ObterOrder",
                new{id = order.OrderId}, order);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }
    }
}