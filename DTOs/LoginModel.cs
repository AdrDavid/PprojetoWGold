using System.ComponentModel.DataAnnotations;

namespace ApiWow.DTOs
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email e obrigatorios")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Senha e obrigatoria")]

        //public string Role { get; set; } = "UsuarioBase";
        public string? Password { get; set; }
    }
}
