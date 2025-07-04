using System.ComponentModel.DataAnnotations;

namespace ShipConnect.DTOs.UserDTOs
{
    public class GetUserDTO
    {
        
        public string Name { get; set; } = string.Empty;
        
        public string Phone { get; set; } = string.Empty;

       
        public string Email { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
