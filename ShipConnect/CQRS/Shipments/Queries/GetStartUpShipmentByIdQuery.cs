using ShipConnect.DTOs.ShipmentDTOs;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class GetStartUpShipmentByIdQuery:IRequest<GeneralResponse<GetShipmentByIdDTO>>
    {
        public string UserId { get; set; }
        public int ShipmentId { get; set; }
    }
    public class GetShipmentByIdQueryHandler : IRequestHandler<GetStartUpShipmentByIdQuery, GeneralResponse<GetShipmentByIdDTO>>
    {
        private readonly IUnitOfWork UnitOfWork;

        public GetShipmentByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<GetShipmentByIdDTO>> Handle(GetStartUpShipmentByIdQuery request, CancellationToken cancellationToken)
        {
            var startUp = await UnitOfWork.StartUpRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserId);

            if (startUp == null)
                return GeneralResponse<GetShipmentByIdDTO>.FailResponse("Startup not found");

            var shipment = await UnitOfWork.ShipmentRepository.GetFirstOrDefaultAsync(sh => sh.Id == request.ShipmentId && sh.StartupId == startUp.Id);

            if (shipment == null)
                return GeneralResponse<GetShipmentByIdDTO>.FailResponse($"Shipment with ID {request.ShipmentId} not found");

            var acceptedOffer = UnitOfWork.OfferRepository.GetWithFilterAsync(o => o.ShipmentId == shipment.Id && o.IsAccepted == true).FirstOrDefault() ;

            var receiverData = await UnitOfWork.ReceiverRepository.GetFirstOrDefaultAsync(r => r.Id == shipment.ReceiverId);

            var data = new GetShipmentByIdDTO
            {
                Id = shipment.Id,
                Code = shipment.Code,
                RequestDate = shipment.CreatedAt,
                WeightKg = shipment.WeightKg,
                Quantity = shipment.Quantity,
                Price = shipment.Price,
                Dimensions = shipment.Dimensions??"N/A",
                ShipmentType = shipment.ShipmentType,
                Status = shipment.Status.ToString(),
                Packaging = shipment.Packaging??"N/A",
                PackagingOptions = shipment.PackagingOptions.ToString(),
                Description = shipment.Description??"N/A",
                DestinationAddress = shipment.DestinationAddress,
                ShippingScope = shipment.ShippingScope.ToString(),
                TransportType = shipment.TransportType.ToString(),
                RequestedPickupDate = shipment.RequestedPickupDate,
                OffersCount = UnitOfWork.OfferRepository.GetWithFilterAsync(o => o.ShipmentId == shipment.Id).Count(),
                SenderPhone = shipment.SenderPhone??"N/A",
                SenderAddress = shipment.SenderAddress??"N/A",
                SentDate = shipment.SentDate,
                ReceiverName = receiverData.FullName,
                ReceiverEmail = receiverData.Email??"N/A",
                ReceiverPhone = receiverData.Phone,
                ActualDelivery = shipment?.ActualDelivery,
                CompanyName= acceptedOffer?.ShippingCompany?.CompanyName??"N/A",
            };

            return GeneralResponse<GetShipmentByIdDTO>.SuccessResponse($"Shipment with ID {request.ShipmentId} retrieved successfully",data);
        }
    }
}
    
