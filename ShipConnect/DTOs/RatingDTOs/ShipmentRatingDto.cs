using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShipConnect.Models;

namespace ShipConnect.DTOs.RatingDTOs
{
    public class ShipmentRatingDto:CompanyRatesDTO
    {
        public int ShipmentId { get; set; }

    }
}
