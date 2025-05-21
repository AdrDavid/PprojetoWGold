using ApiWgold.Models;

namespace ApiWow.DTOs
{
    public class ServerDTO
    {
        public int ServerId { get; set; }
        public string? ServerName { get; set; }
        public GameDTO? Game { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
