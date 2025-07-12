using System.ComponentModel.DataAnnotations;

namespace ShipConnect.DTOs.RatingDTOs
{
    public class CreateRatingDto
    {
        public int OfferId { get; set; }

        [Required]
        [Range(1, 5)]
        public double Score { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }
    }

}
