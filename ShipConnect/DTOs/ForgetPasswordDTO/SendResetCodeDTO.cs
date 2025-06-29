using System.ComponentModel.DataAnnotations;

namespace ShipConnect.DTOs.ResetCodeDTO
{
    public class SendResetCodeDTO
    {
        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
    }
}
