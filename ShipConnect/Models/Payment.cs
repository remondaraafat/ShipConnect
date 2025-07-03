using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Hosting;
using ShipConnect.Enums;
using static ShipConnect.Enums.Enums;

namespace ShipConnect.Models
{
    public class Payment:BaseEntity
    {
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.BankTransfer;
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public string? Currency { get; set; } = "EGP";
        public string? Notes { get; set; }
        //public string PayerId { get; set; }
        //public string PayeeId { get; set; }
        //public int ShipmentId { get; set; }
        public int OfferId { get; set; }
        public bool IsConfirmed { get; set; } //في عملية تأكيد يدوي من الطرف المستلم.

        public int SenderBankAccountId { get; set; }
        [ForeignKey(nameof(SenderBankAccountId))]
        public BankAccount SenderBankAccount { get; set; }
        public int ReceiverBankAccountId { get; set; }

        [ForeignKey(nameof(ReceiverBankAccountId))]
        public BankAccount ReceiverBankAccount { get; set; }
        //[ForeignKey(nameof(PayerId))]
        //[InverseProperty(nameof(ApplicationUser.PaymentsMade))]
        //public ApplicationUser Payer { get; set; }  // اللي دفع

        //[ForeignKey(nameof(PayeeId))]
        //[InverseProperty(nameof(ApplicationUser.PaymentsReceived))]
        //public ApplicationUser Payee { get; set; }  // اللي استلم الفلوس

        //[ForeignKey(nameof(ShipmentId))]
        //public Shipment? Shipment { get; set; }
    }
}
