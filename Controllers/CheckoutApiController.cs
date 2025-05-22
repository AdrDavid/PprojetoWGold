using ApiWgold.Context;
using ApiWgold.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;

namespace ApiWow.Controllers
{
    [Route("create-checkout-session")]
    [ApiController]
    public class CheckoutApiController : Controller
    {
        private readonly AppDbContext _context;

        public CheckoutApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("start")]
        public ActionResult StartCheckout(int goldListingId, int buyerId, int quantity)
        {
            // 1. Buscar o anúncio
            var goldListing = _context.GoldListing.FirstOrDefault(g => g.GoldListingId == goldListingId);
            if (goldListing == null)
                return NotFound("GoldListing não encontrado");

            //busca o vendedor
            var saller = _context.User.FirstOrDefault(u => u.UserId == goldListing.UserId);
            if (saller == null || string.IsNullOrEmpty(saller.ChaveVendedor))
                return NotFound("Vendedor não encontrado");

            // 2. Calcular o valor total
            var total = goldListing.PricePerK * (quantity / 1000);

            // 3. Criar o pedido (Order)
            var order = new Order
            {
                BuyerId = buyerId,
                GoldListingId = goldListingId,
                Quantity = quantity,
                TotalPrice = total,
                Status = "pending",
                CreatedAt = DateTime.UtcNow
            };
            _context.Order.Add(order);
            _context.SaveChanges();

            //comissao
            var applicationFee = (long)(total * 100 * 0.10m);

            // 4. Criar a sessão Stripe
            var domain = "http://localhost:7199";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(total * 100), // Stripe espera valor em centavos
                            Currency = "brl",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Gold {quantity} - {goldListing.Faccao}"
                            }
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = domain + "?success=true&orderId=" + order.OrderId,
                CancelUrl = domain + "?canceled=true&orderId=" + order.OrderId,
                PaymentIntentData = new SessionPaymentIntentDataOptions
                { 
                    ApplicationFeeAmount = applicationFee,
                    TransferData = new SessionPaymentIntentDataTransferDataOptions
                    {
                        Destination = saller.ChaveVendedor 
                    }
                }
            };
            var service = new SessionService();
            Session session = service.Create(options);

            // 5. Retornar a URL do checkout
            return Ok(new { url = session.Url, orderId = order.OrderId });
        }
    }

    [Route("stripe/webhook")]
    [ApiController]
    public class StripeWebhookController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StripeWebhookController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                "whsec_e56554513910a9bf260868cba011f51f2fc2b60a56e22ffc5edf4cccf8346bb2"
            );

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Session;
                // Recupere o orderId da SuccessUrl ou dos metadados da sessão
                var orderId = int.Parse(session.SuccessUrl.Split("orderId=")[1]);
                var order = _context.Order.FirstOrDefault(o => o.OrderId == orderId);

                if (order != null && order.Status != "completed")
                {
                    var goldListing = _context.GoldListing.FirstOrDefault(gl =>
                    gl.GoldListingId == order.GoldListingId);

                    if(goldListing != null && goldListing.Qtd >= order.Quantity)
                    {
                        goldListing.Qtd -= order.Quantity;
                        order.Status = "completed";
                        _context.SaveChanges();
                    }
                }
            }

            return Ok();
        }
    }
}