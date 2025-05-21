using ApiWow.Validations;
using System.ComponentModel.DataAnnotations;

namespace ApiWow.DTOs
{
    public class UserCreateDTO
    {
        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        [PrimeiraLetraMaiuscula]
        public string? Username { get; set; }
        [Required(ErrorMessage = "O email é obrigatório.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "O email é obrigatório.")]
        public string? Password { get; set; }
    }
}
