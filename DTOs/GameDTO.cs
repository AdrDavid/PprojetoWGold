using ApiWgold.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiWow.DTOs
{
    public class GameDTO
    {
        [Key]
        public int GameId { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
