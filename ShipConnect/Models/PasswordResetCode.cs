using System.ComponentModel.DataAnnotations;

namespace ShipConnect.Models
{
    public class PasswordResetCode:BaseEntity
    {
        [Required]
        [EmailAddress]
        public string Email {  get; set; }

        [Required]
        [MaxLength(6)]
        public string Code { get; set; }

        public DateTime ExpirationDate { get; set; }
       
        public bool IsUsed { get; set; } = false;

        //public int Attempts { get; set; } = 0

    }
}
