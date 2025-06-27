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

        [InverseProperty(nameof(Rating.User))]
        public ICollection<Rating> User { get; set; } = new List<Rating>();// اللي عمل التقييم
        [InverseProperty(nameof(Rating.RatedUser))]
        public ICollection<Rating> RatedUser { get; set; } = new List<Rating>();// اللي اتقيم

        [InverseProperty(nameof(Payment.Payer))]
        public ICollection<Payment> PaymentsMade { get; set; } = new List<Payment>();

        [InverseProperty(nameof(Payment.Payee))]
        public ICollection<Payment> PaymentsReceived { get; set; } = new List<Payment>();

        [InverseProperty(nameof(ChatMessage.Sender))]
        public ICollection<ChatMessage> SentMessages { get; set; } = new List<ChatMessage>();

        [InverseProperty(nameof(ChatMessage.Receiver))]
        public ICollection<ChatMessage> ReceivedMessages { get; set; } = new List<ChatMessage>();

        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
        public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    }
}
