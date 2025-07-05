using ShipConnect.DTOs.ShipmentDTOs;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class GetShippingMethodCountQuery : IRequest<GeneralResponse<GetShippingMethodCountDTO>>
    {
        public string UserId { get; set; }
    }

    public class GetShippingMethodCountQueryHandler : IRequestHandler<GetShippingMethodCountQuery, GeneralResponse<GetShippingMethodCountDTO>>
    {
        public IUnitOfWork UnitOfWork { get; }

        public GetShippingMethodCountQueryHandler(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<GetShippingMethodCountDTO>> Handle(GetShippingMethodCountQuery request, CancellationToken cancellationToken)
        {
            var startUp = await UnitOfWork.StartUpRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserId);

            if (startUp == null)
                return GeneralResponse<GetShippingMethodCountDTO>.FailResponse("Startup not found");

            var shipment = UnitOfWork.ShipmentRepository.GetWithFilterAsync(s => s.StartupId == startUp.Id);
            if (shipment == null)
                return GeneralResponse<GetShippingMethodCountDTO>.FailResponse("You didn't add any shipment");

            var data = new GetShippingMethodCountDTO
            {
                Land = shipment.Count(c => c.TransportType == TransportType.Land),
                Sea = shipment.Count(c => c.TransportType == TransportType.Sea),
                Air = shipment.Count(c => c.TransportType == TransportType.Air),
                TotalCount = shipment.Count()
            };

            return GeneralResponse<GetShippingMethodCountDTO>.SuccessResponse("All Shipping Method count retrieved successfully", data);
        }
    }
}
