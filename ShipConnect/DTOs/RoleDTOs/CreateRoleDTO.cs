using System.ComponentModel.DataAnnotations;

namespace ShipConnect.DTOs.RoleDTOs
{
    public class CreateRoleDTO
    {
        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }
    }
}
