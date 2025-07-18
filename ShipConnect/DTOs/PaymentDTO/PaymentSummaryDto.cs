namespace ShipConnect.DTOs.PaymentDTO
{
    public class PaymentSummaryDto
    {
        public int TotalPayments { get; set; }
        public int CompletedPayments { get; set; }
        public int FailedOrPendingPayments { get; set; }
    }
}
