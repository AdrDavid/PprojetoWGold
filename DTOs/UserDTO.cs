using ApiWow.Validations;
using System.ComponentModel.DataAnnotations;

namespace ApiWow.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ChaveVendedor { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
