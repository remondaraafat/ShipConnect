using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using static ShipConnect.Enums.Enums;

namespace ShipConnect.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string CompanyName {  get; set; }
        public string ProfileImageUrl { get; set; }="/images/default-user.png";//default image
        public bool IsActive { get; set; } = true;  //لو الادمن عاوز يعمل تعطيل لحساب المستخدم
        public StartUp? Startup { get; set; }

        public bool AcceptTerms { get; set; } = false;

        public ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
        public ShippingCompany? ShippingCompany { get; set; }

        [InverseProperty(nameof(ChatMessage.Sender))]
        public ICollection<ChatMessage> SentMessages { get; set; }

        [InverseProperty(nameof(ChatMessage.Receiver))]
        public ICollection<ChatMessage> ReceivedMessages { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        //[InverseProperty(nameof(Payment.Payer))]
        //public ICollection<Payment> PaymentsMade { get; set; } = new List<Payment>();

        //[InverseProperty(nameof(Payment.Payee))]
        //public ICollection<Payment> PaymentsReceived { get; set; } = new List<Payment>();
    }
}
