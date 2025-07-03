
using System.ComponentModel.DataAnnotations;

namespace ShipConnect.DTOs.RoleDTOs
{
    public class AssignRoleToUserDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "RoleName is required")]
        [EnumDataType(typeof(UserRole), ErrorMessage = "Invalid RoleName value")]
        public UserRole RoleName { get; set; }
    }
}
