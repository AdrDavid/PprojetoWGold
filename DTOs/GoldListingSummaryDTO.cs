namespace ApiWow.DTOs
{
    public class GoldListingSummaryDTO
    {
        public int GoldListingId { get; set; }
        public decimal PricePerK { get; set; }
        public string? Faccao { get; set; }
        public UserSummaryDTO? User { get; set; }
        public ServerSummaryDTO? Server { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
