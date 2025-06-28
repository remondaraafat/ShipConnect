 using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipConnect.Models
{
    public class BankAccount:BaseEntity
    {
        [MaxLength(100)]
        public string BankName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string AccountNumber { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? IBAN { get; set; }

        [MaxLength(20)]
        public string? SwiftCode { get; set; }//مفيد في التحويلات الدولية

        [MaxLength(150)]
        public string BeneficiaryName { get; set; } = string.Empty;// اسم صاحب الحساب (مثلاً: شركة النقل السريع)

        [MaxLength(10)]
        public string Currency { get; set; }
        public bool IsPrimary { get; set; } = false;// هل ده هو الحساب البنكي الأساسي
<<<<<<< HEAD

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }        // اللي هيوصله الإشعار
        public ICollection<Payment> PaymentsSent { get; set; }
        public ICollection<Payment> PaymentsReceived { get; set; }
        //public int? ShippingCompanyId { get; set; }
        //public int? StartUpId { get; set; }

=======
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }        // اللي هيوصله الإشعار
        public ICollection<Payment> PaymentsSent { get; set; }
        public ICollection<Payment> PaymentsReceived { get; set; }
        //public int? ShippingCompanyId { get; set; }
        //public int? StartUpId { get; set; }

>>>>>>> a242051c9ffedfca8cd88739cf07ebd2e219a7fa
        //[ForeignKey(nameof(ShippingCompanyId))]
        //public ShippingCompany? ShippingCompany { get; set; }

        //[ForeignKey(nameof(StartUpId))]
        //public StartUp? StartUp { get; set; }
    }
}
