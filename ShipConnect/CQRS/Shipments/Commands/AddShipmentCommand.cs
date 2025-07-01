using MediatR;
using ShipConnect.Helpers;
using static ShipConnect.Enums.Enums;

namespace ShipConnect.CQRS.Shipments.Commands
{
    public class AddShipmentCommand:IRequest<GeneralResponse<string>>
    {
        public string Title { get; set; }
        public double WeightKg { get; set; }  // وزن الشحنة بالكيلو
        public string? Dimensions { get; set; }
        public int Quantity { get; set; }
        public decimal MinPrice { get; set; }
        public string DestinationAddress { get; set; }
        public string DestinationCity { get; set; }
        public string ShipmentType { get; set; }
        public TransportType TransportType { get; set; }
        public ShippingScope ShippingScope { get; set; }
        public string? Packaging { get; set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
        public DateTime RequestedPickupDate { get; set; }//تاريخ الاستلام المطلوب

        //startUp data
        public string SenderPhone { get; set; }
        public string SenderAddress { get; set; }//عنوان الارسال
        public string SenderCity { get; set; }
        public DateTime? SentDate { get; set; }//تاريخ الارسال

        //recipient data
        public string RecipientName { get; set; } = string.Empty;
        public string? RecipientEmail { get; set; }
        public string RecipientPhone { get; set; } = string.Empty;
        public string? ReceiverNotes { get; set; }

        public int StartupId { get; set; }

    }
}
