
using System.ComponentModel.DataAnnotations;

namespace ShipConnect.DTOs.RoleDTOs
{
    public class AssignRoleToUserDTO
    {
        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; } = null!;

        [Required(ErrorMessage = "RoleName is required")]
        [EnumDataType(typeof(UserRole), ErrorMessage = "Invalid RoleName value")]
        public UserRole RoleName { get; set; }
    }
}
