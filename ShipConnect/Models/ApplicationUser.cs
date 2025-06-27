using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using static ShipConnect.Enums.Enums;

namespace ShipConnect.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string ProfileImageUrl { get; set; }="/images/default-user.png";//default image
        public bool IsActive { get; set; } = true;  //لو الادمن عاوز يعمل تعطيل لحساب المستخدم
        public UserRole Role { get; set; }
        public StartUp? StartupProfile { get; set; }
        public ShippingCompany? ShippingCompanyProfile { get; set; }

        [InverseProperty("User")]
        public ICollection<Rating> User { get; set; } = new List<Rating>();// اللي عمل التقييم
        [InverseProperty("RatedUser")]
        public ICollection<Rating> RatedUser { get; set; } = new List<Rating>();// اللي اتقيم

        public ICollection<Payment> PaymentsMade { get; set; } = new List<Payment>();
        public ICollection<Payment> PaymentsReceived { get; set; } = new List<Payment>();

        public ICollection<ChatMessage> SentMessages { get; set; } = new List<ChatMessage>();
        public ICollection<ChatMessage> ReceivedMessages { get; set; } = new List<ChatMessage>();

        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
        public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    }
}
