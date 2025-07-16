namespace ShipConnect.DTOs.PaymentDTO
{
    public class PayPalPaymentRequestDto
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EGP";
        public int OfferId { get; set; }
        public string Notes { get; set; } = "";
        public string PayerId { get; set; }
        public string PayPalEmail { get; set; } = ""; // ← جديد
    }

}
