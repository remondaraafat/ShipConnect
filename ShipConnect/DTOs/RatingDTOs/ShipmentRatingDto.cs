using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShipConnect.Models;

namespace ShipConnect.DTOs.RatingDTOs
{
    public class ShipmentRatingDto
    {
        public int ShipmentId { get; set; }
        public string ShipmentCode { get; set; } 
        public double? Score { get; set; }
        public string? Comment { get; set; }
        public string StartUpName { get; set; }
        public string ImageUrl { get; set; }
        public DateTime RatedAt { get; set; }

    }
}
