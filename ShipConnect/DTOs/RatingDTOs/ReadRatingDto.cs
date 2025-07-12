namespace ShipConnect.DTOs.RatingDTOs
{
    public class ReadRatingDto
    {
        public int Id { get; set; }
        public int StartUpId { get; set; }
        public int CompanyId { get; set; }
        public int OfferId { get; set; }
        public double Score { get; set; }
        public string? Comment { get; set; }
    }
}
