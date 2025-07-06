using ShipConnect.DTOs.ShipmentDTOs;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class AdminStartUpStatusCountQuery : IRequest<GeneralResponse<GetAllStatusCountDTO>>
    {
        public int StartUpID { get; set; }
    }

    public class AdminStartUpStatusCountQueryHandler : IRequestHandler<AdminStartUpStatusCountQuery, GeneralResponse<GetAllStatusCountDTO>>
    {
        public IUnitOfWork UnitOfWork { get; }

        public AdminStartUpStatusCountQueryHandler(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<GetAllStatusCountDTO>> Handle(AdminStartUpStatusCountQuery request, CancellationToken cancellationToken)
        {
            var startUp = await UnitOfWork.StartUpRepository.GetFirstOrDefaultAsync(s => s.Id == request.StartUpID);

            if (startUp == null)
                return GeneralResponse<GetAllStatusCountDTO>.FailResponse("Startup not found");

            var shipment = UnitOfWork.ShipmentRepository.GetWithFilterAsync(s => s.StartupId == startUp.Id);
            if (shipment == null)
                return GeneralResponse<GetAllStatusCountDTO>.FailResponse("You didn't add any shipment");

            var data = new GetAllStatusCountDTO
            {
                Pending = shipment.Count(c => c.Status == ShipmentStatus.Pending),
                Preparing = shipment.Count(c => c.Status == ShipmentStatus.Preparing),
                InTransit = shipment.Count(c => c.Status == ShipmentStatus.InTransit),
                AtWarehouse = shipment.Count(c => c.Status == ShipmentStatus.AtWarehouse),
                OutForDelivery = shipment.Count(c => c.Status == ShipmentStatus.OutForDelivery),
                Delivered = shipment.Count(c => c.Status == ShipmentStatus.Delivered),
                Failed = shipment.Count(c => c.Status == ShipmentStatus.Failed),
                TotalCount = shipment.Count()
            };

            return GeneralResponse<GetAllStatusCountDTO>.SuccessResponse("All Shipments status count retrieved successfully", data);
        }
    }

}
