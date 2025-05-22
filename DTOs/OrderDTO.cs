using ApiWgold.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiWow.DTOs
{
    public class OrderDTO
    {
        public int OrderId { get; set; }

        public int BuyerId { get; set; }
    
        public int GoldListingId { get; set; }

        public string? CharName { get; set; }

        public GoldListingSummaryDTO? GoldListing { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        // Valor total da compra
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        // Status do pedido
        [StringLength(30)]
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
