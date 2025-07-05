namespace ShipConnect.DTOs.ShipmentDTOs
{
    public class GetAllStatusCountDTO
    {
        public int Pending { get; set; }
        public int Preparing { get; set; }
        public int InTransit {get;set;}
        public int AtWarehouse { get; set; }
        public int OutForDelivery { get; set; }
        public int Delivered { get; set; }
        public int Failed { get; set; }

        public int TotalCount { get; set; }
    }
}
