using ShipConnect.DTOs.ShipmentDTOs;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class GetShippingShipmentByIdQuery : IRequest<GeneralResponse<GetShipmentByIdDTO>>
    {
        public string UserId { get; set; }
        public int ShipmentId { get; set; }
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
                return GeneralResponse<GetShipmentByIdDTO>.FailResponse("Shipping Company not found");

            var offer = UnitOfWork.OfferRepository
                                .GetWithFilterAsync(o=>o.ShipmentId==request.ShipmentId && o.ShippingCompanyId==company.Id)
                                .Where(o=>o.IsAccepted==true)
                                .Select(o=> new {o.Shipment, o.Shipment.Startup, o.Shipment.Receiver})
                                .FirstOrDefault();

            if (offer == null || offer.Shipment==null)
                return GeneralResponse<GetShipmentByIdDTO>.FailResponse($"Shipment with ID {request.ShipmentId} not found");

            var shipment = offer.Shipment;
            var reciever = offer.Shipment.Receiver;
            var startUp = offer.Shipment.Startup;

            var data = new GetShipmentByIdDTO
            {
                Id = shipment.Id,
                Code = shipment.Code,
                RequestDate = shipment.CreatedAt,
                WeightKg = shipment.WeightKg,
                Quantity = shipment.Quantity,
                Price = shipment.Price,
                Dimensions = shipment.Dimensions ?? "N/A",
                ShipmentType = shipment.ShipmentType,
                Status = shipment.Status.ToString(),
                Packaging = shipment.Packaging ?? "N/A",
                PackagingOptions = shipment.PackagingOptions.ToString(),
                Description = shipment.Description ?? "N/A",
                DestinationAddress = shipment.DestinationAddress,
                ShippingScope = shipment.ShippingScope.ToString(),
                TransportType = shipment.TransportType.ToString(),
                RequestedPickupDate = shipment.RequestedPickupDate,
                OffersCount = UnitOfWork.OfferRepository.GetWithFilterAsync(o => o.ShipmentId == shipment.Id).Count(),
                SenderPhone = shipment.SenderPhone ?? "N/A",
                SenderAddress = shipment.SenderAddress ?? "N/A",
                SentDate = shipment.SentDate,
                ReceiverName = reciever.FullName,
                ReceiverEmail = reciever?.Email ?? "N/A",
                ReceiverPhone = reciever?.Phone,
                
                ActualDelivery = shipment?.ActualDelivery,
                CompanyName = startUp.CompanyName,
            };

            return GeneralResponse<GetShipmentByIdDTO>.SuccessResponse($"Shipment with ID {request.ShipmentId} retrieved successfully", data);
        }
    }
}

