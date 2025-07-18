using System.ComponentModel.DataAnnotations;

namespace ShipConnect.DTOs.StartUpDTOs
{
    public class GetStartupProfileDTO
    {
        public int Id { get; set; } 
        public string StartupName { get; set; } = string.Empty;
        public string Email { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? BusinessCategory { get; set; }
        public string? Description { get; set; }
        public string? Website { get; set; }
        public string? TaxId { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
