using System.ComponentModel.DataAnnotations;

namespace ShipConnect.DTOs.RatingDTOs
{
    public class UpdateRatingDto
    {
        [Range(1, 5)]
        public int Score { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }
    }
}
