using ApiWow.Validations;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiWgold.Models
{
    public class User
    {

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public User()
        {
            Order = new Collection<Order>();
            GoldListing = new Collection<GoldListing>();
        }
        //[JsonIgnore]
        public int UserId { get; set; }

        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        [PrimeiraLetraMaiuscula]
        public string? Username { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        public string? Email { get; set; }
        public string Role { get; set; } = "UsuarioBase";
        public string? Password { get; set; }
        public string? ChaveVendedor { get; set; }
        //[JsonIgnore]
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public ICollection<Order>? Order { get; set; }
        [JsonIgnore]
        public ICollection<GoldListing>? GoldListing { get; set; }
    }
}