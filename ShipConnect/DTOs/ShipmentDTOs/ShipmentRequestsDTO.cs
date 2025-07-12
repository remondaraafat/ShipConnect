using ShipConnect.Models;

namespace ShipConnect.DTOs.ShipmentDTOs
{
    public class ShipmentRequestsDTO
    {
        public int ShipmentId { get; set; }
        public string StartupName { get; set; }
        public string ShipmentType { get; set; } // نوع الشحنة (مثلاً: أجهزة، أوراق، ...)
        public string? SenderAddress { get; set; }//عنوان الارسال
        public string DestinationAddress { get; set; }
        public DateTime SentDate { get; set; }//تاريخ الارسال
    }
}
