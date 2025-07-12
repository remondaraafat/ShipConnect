using System.Text.RegularExpressions;
using ShipConnect.DTOs.ShipmentDTOs;
using ShipConnect.Models;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class GetShipmentDetailsQuery : IRequest<GeneralResponse<ShipmentDetailsDTO>>
    {
        public string UserId { get; }
        public int ShipmentId { get; }

        public GetShipmentDetailsQuery(string userId, int shipmentId)
        {
            UserId = userId;
            ShipmentId = shipmentId;
        }
    }

    public class GetShipmentDetailsQueryHandler : IRequestHandler<GetShipmentDetailsQuery, GeneralResponse<ShipmentDetailsDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetShipmentDetailsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<ShipmentDetailsDTO>> Handle(GetShipmentDetailsQuery request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.ShippingCompanyRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserId);

            if (company == null)
                return GeneralResponse<ShipmentDetailsDTO>.FailResponse("User not found");

            var shipment = await _unitOfWork.ShipmentRepository
                                            .GetWithFilterAsync(s=>s.Id == request.ShipmentId
                                            && s.Status==ShipmentStatus.Pending)
                                            .Select(s=> new ShipmentDetailsDTO
                                            {
                                                Id = s.Id,
                                                Code = s.Code,
                                                RequestDate = s.CreatedAt,
                                                WeightKg = s.WeightKg,
                                                Quantity = s.Quantity,
                                                Price = s.Price,
                                                Dimensions = s.Dimensions?? "N/A",
                                                ShipmentType = s.ShipmentType,
                                                Status = Regex.Replace(s.Status.ToString(), "(\\B[A-Z])", " $1"), // من Pending إلى "Pending", FromOutForDelivery إلى "Out For Delivery"
                                                Packaging = s.Packaging?? "N/A",
                                                PackagingOptions = Regex.Replace(s.PackagingOptions.ToString(), "(\\B[A-Z])", " $1"),
                                                Description = s.Description?? "N/A",    
                                                DestinationAddress = s.DestinationAddress,
                                                ShippingScope=s.ShippingScope.ToString(),
                                                TransportType = s.TransportType.ToString(),
                                                RequestedPickupDate= s.RequestedPickupDate,
                                                SenderAddress= s.SenderAddress?? "N/A",
                                                SentDate= s.SentDate,
                                                CompanyName=s.Startup.CompanyName
                                            }).FirstOrDefaultAsync(cancellationToken);

            if (shipment == null)
                return GeneralResponse<ShipmentDetailsDTO>.FailResponse("Shipment not found or access denied");

            return GeneralResponse<ShipmentDetailsDTO>.SuccessResponse($"Shipment with ID {request.ShipmentId} retrieved successfully", shipment);


        }
    }

}
