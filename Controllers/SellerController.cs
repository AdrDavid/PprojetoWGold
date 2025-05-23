//using Microsoft.AspNetCore.Components;
using ApiWgold.Context;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ApiWow.Controllers
{
    [Route("api/seller")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SellerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("onbording/{userId}")]
        public async Task<ActionResult> StartOnboarding(int userId)
        {
            var user = _context.User.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            //criacao da conta conectada com a Stripe

            var accountOptions = new AccountCreateOptions
            {
                Type = "express",
                Country = "BR",
                Email = user.Email
            };

            var accountService = new AccountService();
            var account = await accountService.CreateAsync(accountOptions);

            //salvar o accont.Id no campo ChaveVendedor
            user.ChaveVendedor = account.Id;
            user.Role = "Aguardando_Aprovacao";
            _context.SaveChanges();

            // gerar o link de onboarding
            var accountLinkOptions = new AccountLinkCreateOptions
            {
                Account = account.Id,
                RefreshUrl = "http://localhost:5000/vender",
                ReturnUrl = "http://localhost:5000/vender",
                Type = "account_onboarding",
            };

            var accountLinkService = new AccountLinkService();
            var accountLink = await accountLinkService.CreateAsync(accountLinkOptions);
            
           
            //retornar o link pro front redirecionar o usuario
            return Ok(new {onBordingUrl = accountLink.Url});
        }


    }
}
