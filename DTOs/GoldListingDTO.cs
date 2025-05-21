using ApiWgold.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiWow.DTOs
{
    public class GoldListingDTO
    {
        public int GoldListingId { get; set; }
        public decimal PricePerK { get; set; }
        public int Qtd { get; set; }
        public string? Faccao { get; set; }
        public UserSummaryDTO? User { get; set; }
        public ServerSummaryDTO? Server { get; set; }
        public DateTime CreatedAt { get; set; }
       
    }
}
