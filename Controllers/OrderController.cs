using ApiWgold.Context;
using ApiWgold.Models;
using ApiWow.DTOs;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> Get()
        {
            try
            {
                var orders = await _context.Order.AsNoTracking()
                    .Include(o => o.GoldListing)
                        .ThenInclude(g => g.User)
                    .Include(o => o.GoldListing)
                        .ThenInclude(g => g.Server)
                    .Select(o => new OrderDTO
                    {
                        OrderId = o.OrderId,
                        BuyerId = o.BuyerId,
                        CharName = o.CharName,
                        GoldListing = new GoldListingSummaryDTO
                        {
                            GoldListingId = o.GoldListing.GoldListingId,
                            Faccao = o.GoldListing.Faccao,
                            PricePerK = o.GoldListing.PricePerK,
                            CreatedAt = o.GoldListing.CreatedAt,
                            User = o.GoldListing.User == null ? null : new UserSummaryDTO
                            {
                                UserId = o.GoldListing.User.UserId,
                                Username = o.GoldListing.User.Username
                            },
                            Server = o.GoldListing.Server == null ? null : new ServerSummaryDTO
                            {
                                ServerId = o.GoldListing.Server.ServerId,
                                ServerName = o.GoldListing.Server.ServerName // ou o campo correto do nome do servidor
                            }
                        },
                        Quantity = o.Quantity,
                        TotalPrice = o.TotalPrice,
                        Status = o.Status,
                        CreatedAt = o.CreatedAt
                    }).ToListAsync();

                return Ok(orders);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }


        [HttpGet("{id:int}", Name = "ObterOrder")]
        [Authorize]
        public async Task<ActionResult<OrderDTO>> Get(int id)
        {
            try
            {
                var orders = await _context.Order.AsNoTracking()
                     .Where(o => o.OrderId == id)
                     .Include(o => o.GoldListing)
                         .ThenInclude(g => g.User)
                     .Include(o => o.GoldListing)
                         .ThenInclude(g => g.Server)
                     .Select(o => new OrderDTO
                     {
                         OrderId = o.OrderId,
                         BuyerId = o.BuyerId,
                         CharName = o.CharName,
                         GoldListing = new GoldListingSummaryDTO
                         {
                             GoldListingId = o.GoldListing.GoldListingId,
                             Faccao = o.GoldListing.Faccao,
                             PricePerK = o.GoldListing.PricePerK,
                             CreatedAt = o.GoldListing.CreatedAt,
                             User = o.GoldListing.User == null ? null : new UserSummaryDTO
                             {
                                 UserId = o.GoldListing.User.UserId,
                                 Username = o.GoldListing.User.Username
                             },
                             Server = o.GoldListing.Server == null ? null : new ServerSummaryDTO
                             {
                                 ServerId = o.GoldListing.Server.ServerId,
                                 ServerName = o.GoldListing.Server.ServerName // ou o campo correto do nome do servidor
                             }
                         },
                         Quantity = o.Quantity,
                         TotalPrice = o.TotalPrice,
                         Status = o.Status,
                         CreatedAt = o.CreatedAt
                     }).FirstOrDefaultAsync();
                return Ok(orders);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro de Servidor Contate o DEPARTAMENTO DE TI");
            }
        }


        [HttpPost]
        [Authorize]
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