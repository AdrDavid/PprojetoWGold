using System.Collections.ObjectModel;

namespace ApiWgold.Models
{
    public class Server
    
    {

        public Server()
        {
            GoldListings = new Collection<GoldListing>();
        }
        public int ServerId { get; set; }

        public string? Servers { get; set; }

        public int GamesId { get; set; }
        public Game? Games { get; set; }

        public ICollection<GoldListing>? GoldListings { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}