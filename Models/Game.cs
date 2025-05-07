using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiWgold.Models
{
    [Table("Game")]
    public class Game
    {
        public Game()
        {
            Server = new Collection<Server>();
        }
        [Key]
        public int GameId { get; set; }

        [Required][StringLength(100)]
        public string? Name { get; set; }
        public ICollection<Server>? Server { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}