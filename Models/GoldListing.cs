using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int GoldListingId { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int ServerId { get; set; }
        public Server? Server { get; set; }
        
        [Required][Column(TypeName = "decimal(10,2)")]
        public decimal PricePerK { get; set; }
        public int Qtd { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Order>? Order { get; set; }
    }
}