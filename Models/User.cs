using System.Collections.ObjectModel;

namespace ApiWgold.Models
{
    public class User
    {

        public User()
        {
            Order = new Collection<Order>();
            GoldListing = new Collection<GoldListing>();
        }
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<Order>? Order { get; set; }
        public ICollection<GoldListing>? GoldListing { get; set; }
    }
}