using System.ComponentModel.DataAnnotations;

namespace ApiWow.DTOs
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Nome De Usuario e obrigatorios")]
        public string? UserName { get; set; }

        [EmailAddress(ErrorMessage = "Email invalido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Senha e obrigatoria")]
        public string? Password { get; set; }
    }
}
