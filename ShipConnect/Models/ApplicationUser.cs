using Microsoft.AspNetCore.Identity;

namespace ShipConnect.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string ProfileImageUrl { get; set; }="/images/default-user.png";//default image
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;//date time now
        public bool IsActive { get; set; } = true;  //لو الادمن عاوز يعمل تعطيل لحساب المستخدم
    
    }
}
