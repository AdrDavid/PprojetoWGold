using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiWgold.Models
{
    [Table("GoldListing")]
    public class GoldListing
    {
        public GoldListing()
        {
            Order = new Collection<Order>();
        }
        [Key]
        [JsonIgnore]
        public int GoldListingId { get; set; }
        public int UserId { get; set; }
        
        [JsonIgnore]
        public User? User { get; set; }
        public int ServerId { get; set; }
        [JsonIgnore]
        public Server? Server { get; set; }

        [Required][Column(TypeName = "decimal(10,2)")]
        public decimal PricePerK { get; set; }
        public int Qtd { get; set; }
        [Required][StringLength(30)]
        public string? Faccao { get; set; }

        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public ICollection<Order>? Order { get; set; }
    }
}