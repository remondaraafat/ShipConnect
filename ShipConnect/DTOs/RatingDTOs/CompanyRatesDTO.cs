namespace ShipConnect.DTOs.RatingDTOs
{
    public class CompanyRatesDTO
    {
        public double? Score { get; set; }
        public string? Comment { get; set; }
        public string StartUpName { get; set; }
        public string ImageUrl { get; set; }
        public string ShipmentCode { get; set; }
        public DateTime RatedAt { get; set; }

    }
}
