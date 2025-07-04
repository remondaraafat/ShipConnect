using System.ComponentModel.DataAnnotations;

namespace ShipConnect.DTOs.StartUpDTOs
{
    public class EditStartupDTO
    {

        

        [StringLength(1000, ErrorMessage = "Description can't exceed 1000 characters.")]
        public string? Description { get; set; }

        [StringLength(100, ErrorMessage = "Business category can't exceed 100 characters.")]
        public string? BusinessCategory { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, ErrorMessage = "Address can't exceed 200 characters.")]
        public string Address { get; set; } = string.Empty;

        
        [Url(ErrorMessage = "Invalid URL format.")]
        [StringLength(200, ErrorMessage = "Website URL can't exceed 200 characters.")]
        public string? Website { get; set; }
        [Required(ErrorMessage = "Tax ID is required.")]
        [StringLength(20, ErrorMessage = "Tax ID can't exceed 20 characters.")]
        //[RegularExpression(@"^[A-Za-z0-9\-]+$", ErrorMessage = "Tax ID can only contain letters, numbers, and hyphens.")]
        public string? TaxId { get; set; }
        
    }


}

