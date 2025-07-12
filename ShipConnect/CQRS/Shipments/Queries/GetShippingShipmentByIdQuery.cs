using System.Text.RegularExpressions;
using ShipConnect.DTOs.ShipmentDTOs;
using ShipConnect.Models;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class GetShippingShipmentByIdQuery : IRequest<GeneralResponse<GetShipmentByIdDTO>>
    {
        public string UserId { get;}
        public int ShipmentId { get;}

        public GetShippingShipmentByIdQuery(string userId, int shipmentId)
        {
            UserId = userId;
            ShipmentId = shipmentId;
        }
    }
    public class GetShippingShipmentByIdQueryHandler : IRequestHandler<GetShippingShipmentByIdQuery, GeneralResponse<GetShipmentByIdDTO>>
    {
        private readonly IUnitOfWork UnitOfWork;

        public GetShippingShipmentByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<GetShipmentByIdDTO>> Handle(GetShippingShipmentByIdQuery request, CancellationToken cancellationToken)
        {
            var company = await UnitOfWork.ShippingCompanyRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserId);

            if (company == null)
                return GeneralResponse<GetShipmentByIdDTO>.FailResponse("User not found");

            var offer =await  UnitOfWork.OfferRepository
                                .GetWithFilterAsync(o=>o.ShipmentId==request.ShipmentId && o.ShippingCompanyId==company.Id&& o.IsAccepted)
                                .Select(s=> new GetShipmentByIdDTO
                                {
                                    Id = s.Shipment.Id,
                                    Code = s.Shipment.Code,
                                    RequestDate = s.Shipment.CreatedAt,
                                    WeightKg = s.Shipment.WeightKg,
                                    Quantity = s.Shipment.Quantity,
                                    Price = s.Shipment.Price,
                                    Dimensions = s.Shipment.Dimensions ?? "N/A",
                                    ShipmentType = s.Shipment.ShipmentType,
                                    Status = Regex.Replace(s.Shipment.Status.ToString(), "(\\B[A-Z])", " $1"), // من Pending إلى "Pending", FromOutForDelivery إلى "Out For Delivery"
                                    Packaging = s.Shipment.Packaging ?? "N/A",
                                    PackagingOptions = s.Shipment.PackagingOptions.ToString(),
                                    Description = s.Shipment.Description ?? "N/A",
                                    DestinationAddress = s.Shipment.DestinationAddress,
                                    ShippingScope = s.Shipment.ShippingScope.ToString(),
                                    TransportType = s.Shipment.TransportType.ToString(),
                                    RequestedPickupDate = s.Shipment.RequestedPickupDate,
                                    SenderPhone = s.Shipment.SenderPhone ?? "N/A",
                                    SenderAddress = s.Shipment.SenderAddress ?? "N/A",
                                    SentDate = s.Shipment.SentDate,
                                    ReceiverName = s.Shipment.Receiver.FullName,
                                    ReceiverEmail = s.Shipment.Receiver.Email ?? "N/A",
                                    ReceiverPhone = s.Shipment.Receiver.Phone,
                                    offerId = s.Id,
                                    ActualSentDate = s.Shipment.ActualSentDate,
                                    ActualDelivery = s.Shipment.ActualDelivery,
                                    CompanyName = s.Shipment.Startup.CompanyName,
                                    RatingScore = s.Ratings.Score,
                                }).FirstOrDefaultAsync(cancellationToken);

            if (offer == null)
                return GeneralResponse<GetShipmentByIdDTO>.FailResponse("Shipment not found or access denied");

            return GeneralResponse<GetShipmentByIdDTO>.SuccessResponse($"Shipment with ID {request.ShipmentId} retrieved successfully", offer);
        }
    }
}

