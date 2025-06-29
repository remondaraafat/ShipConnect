using System.ComponentModel.DataAnnotations;

namespace ShipConnect.DTOs.AccountDTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage ="*")]
        [EmailAddress(ErrorMessage ="Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
