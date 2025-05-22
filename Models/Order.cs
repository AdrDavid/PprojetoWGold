using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiWgold.Models
{
    [Table("Order")]    
    public class Order
    {
        [Key]
        [JsonIgnore]
        public int OrderId { get; set; }

        // ID do comprador (usuário que está comprando gold)
        public int BuyerId { get; set; }
        [JsonIgnore]
        public User? Buyer { get; set; }

        public string? CharName { get; set; } // Nome do personagem do comprador
        // ID do anúncio de venda de gold
        public int GoldListingId { get; set; }
    
        [JsonIgnore]
        public GoldListing? GoldListing { get; set; }

        // Quantidade comprada em milhares (ex: 3K = 3000 gold)
        [Required][Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        // Valor total da compra
        [Required][Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        // Status do pedido
        [Required][StringLength(30)]
        public string Status { get; set; } = "pending"; // pode ser 'pending', 'completed', 'cancelled'
        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
    }
}