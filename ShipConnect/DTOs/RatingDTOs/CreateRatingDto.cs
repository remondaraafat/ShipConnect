using System.ComponentModel.DataAnnotations;

namespace ShipConnect.DTOs.RatingDTOs
{
    public class CreateRatingDto
    {
        public int StartUpId { get; set; }
        public int CompanyId { get; set; }
        public int OfferId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Score { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }
    }

}
