using System.Text.RegularExpressions;
using ShipConnect.DTOs.ShipmentDTOs;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class GetStartUpShipmentByIdQuery:IRequest<GeneralResponse<GetShipmentByIdDTO>>
    {
        public string UserId { get; }
        public int ShipmentId { get; set; }

        public GetStartUpShipmentByIdQuery(string userId, int shipmentId)
        {
            UserId = userId;
            ShipmentId = shipmentId;
        }
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
                return GeneralResponse<GetShipmentByIdDTO>.FailResponse("User not found");

            var shipment = await UnitOfWork.ShipmentRepository.GetFirstOrDefaultAsync(sh => sh.Id == request.ShipmentId && sh.StartupId == startUp.Id);
            if (shipment == null)
                return GeneralResponse<GetShipmentByIdDTO>.FailResponse($"Shipment with ID {request.ShipmentId} not found");

            var acceptedOffer =await UnitOfWork.OfferRepository
                                            .GetWithFilterAsync(o => o.ShipmentId == shipment.Id && o.IsAccepted == true)
                                            .Include(s=>s.Ratings).Include(s=>s.ShippingCompany)
                                            .FirstOrDefaultAsync(cancellationToken);

            var receiverData = await UnitOfWork.ReceiverRepository.GetFirstOrDefaultAsync(r => r.Id == shipment.ReceiverId);
            if (receiverData == null)
                return GeneralResponse<GetShipmentByIdDTO>.FailResponse("Receiver data not found");

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
                Status = Regex.Replace(shipment.Status.ToString(), "(\\B[A-Z])", " $1"), // من Pending إلى "Pending", FromOutForDelivery إلى "Out For Delivery"
                //Packaging = shipment.Packaging??"N/A",
                PackagingOptions = shipment.PackagingOptions.ToString(),
                Description = shipment.Description??"N/A",
                DestinationAddress = shipment.DestinationAddress,
                ShippingScope = shipment.ShippingScope.ToString(),
                TransportType = shipment.TransportType.ToString(),
                RequestedPickupDate = shipment.RequestedPickupDate,
                OffersCount = await UnitOfWork.OfferRepository.GetWithFilterAsync(o => o.ShipmentId == shipment.Id).CountAsync(cancellationToken),
                SenderPhone = shipment.SenderPhone??"N/A",
                SenderAddress = shipment.SenderAddress??"N/A",
                SentDate = shipment.SentDate,
                ReceiverName = receiverData.FullName,
                ReceiverEmail = receiverData.Email??"N/A",
                ReceiverPhone = receiverData.Phone,
                ActualSentDate = shipment.ActualSentDate,
                ActualDelivery = shipment?.ActualDelivery,
                CompanyName= acceptedOffer?.ShippingCompany?.CompanyName??"N/A",
                offerId = acceptedOffer?.Id??0,
                RatingScore = acceptedOffer?.Ratings?.Score??0,
            };

            return GeneralResponse<GetShipmentByIdDTO>.SuccessResponse($"Shipment with ID {request.ShipmentId} retrieved successfully",data);
        }
    }
}
    
