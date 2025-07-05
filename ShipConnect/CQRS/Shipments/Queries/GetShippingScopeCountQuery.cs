using ShipConnect.DTOs.ShipmentDTOs;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class GetShippingScopeCountQuery : IRequest<GeneralResponse<GetShippingScopeCountDTO>>
    {
        public string UserId { get; set; }
    }

    public class GetShippingScopeCountQueryHandler : IRequestHandler<GetShippingScopeCountQuery, GeneralResponse<GetShippingScopeCountDTO>>
    {
        public IUnitOfWork UnitOfWork { get; }

        public GetShippingScopeCountQueryHandler(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<GetShippingScopeCountDTO>> Handle(GetShippingScopeCountQuery request, CancellationToken cancellationToken)
        {
            var startUp = await UnitOfWork.StartUpRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserId);

            if (startUp == null)
                return GeneralResponse<GetShippingScopeCountDTO>.FailResponse("Startup not found");

            var shipment = UnitOfWork.ShipmentRepository.GetWithFilterAsync(s => s.StartupId == startUp.Id);
            if (shipment == null)
                return GeneralResponse<GetShippingScopeCountDTO>.FailResponse("You didn't add any shipment");

            var data = new GetShippingScopeCountDTO
            {
                Domestic = shipment.Count(c => c.ShippingScope == ShippingScope.Domestic),
                International = shipment.Count(c => c.ShippingScope == ShippingScope.International),
                TotalCount = shipment.Count()
            };

            return GeneralResponse<GetShippingScopeCountDTO>.SuccessResponse("All Shipping Method count retrieved successfully", data);
        }
    }
}
