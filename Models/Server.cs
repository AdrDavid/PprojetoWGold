using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public Game? Games { get; set; }

        [JsonIgnore]
        public ICollection<GoldListing>? GoldListings { get; set; }

        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
    }
}