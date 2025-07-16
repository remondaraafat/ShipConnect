using ShipConnect.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Payment : BaseEntity
{
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.PayPal;
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public string Currency { get; set; } = "USD";
    public string? Notes { get; set; }

    public int OfferId { get; set; }
    public bool IsConfirmed { get; set; } = false;

    public string? PayPalOrderId { get; set; }
    public string? PayPalEmail { get; set; } 

    public string PayerId { get; set; }
    [ForeignKey(nameof(PayerId))]
    public ApplicationUser Payer { get; set; }

    public int? SenderBankAccountId { get; set; }
    public BankAccount? SenderBankAccount { get; set; }

    public int? ReceiverBankAccountId { get; set; }
    public BankAccount? ReceiverBankAccount { get; set; }
}

