using System.ComponentModel.DataAnnotations;

namespace ShipConnect.DTOs.ShipmentDTOs
{
    public class GetShipmentByIdDTO: ShipmentDetailsDTO
    {
        public string? SenderPhone { get; set; }
        public int OffersCount { get; set; }
        public DateTime? ActualSentDate { get; set; }//تاريخ الارسال الفعلي
        public DateTime? ActualDelivery { get; set; }//تاريخ التسليم الفعلي
        public int offerId { get; set; }


        //receiver data
        public string ReceiverName { get; set; } = string.Empty;
        public string ReceiverPhone { get; set; }
        public string? ReceiverEmail { get; set; }

        //rating
        public double? RatingScore { get; set; }

    }
}
