using ShipConnect.DTOs.ShipmentDTOs;

namespace ShipConnect.CQRS.shipments.Queries
{
    public class AdminStatusCountQuery : IRequest<GeneralResponse<GetAllStatusCountDTO>>
    {
    }

    public class AdminStatusCountQueryHandler : IRequestHandler<AdminStatusCountQuery, GeneralResponse<GetAllStatusCountDTO>>
    {
        public IUnitOfWork UnitOfWork { get; }

        public AdminStatusCountQueryHandler(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<GetAllStatusCountDTO>> Handle(AdminStatusCountQuery request, CancellationToken cancellationToken)
        {
            var shipments = UnitOfWork.ShipmentRepository.GetAllAsync().ToList();
            if (shipments == null || !shipments.Any())
                return GeneralResponse<GetAllStatusCountDTO>.FailResponse("No shipments found yet");

            var data = new GetAllStatusCountDTO
            {
                Pending = shipments.Count(c => c.Status == ShipmentStatus.Pending),
                Preparing = shipments.Count(c => c.Status == ShipmentStatus.Preparing),
                InTransit = shipments.Count(c => c.Status == ShipmentStatus.InTransit),
                AtWarehouse = shipments.Count(c => c.Status == ShipmentStatus.AtWarehouse),
                OutForDelivery = shipments.Count(c => c.Status == ShipmentStatus.OutForDelivery),
                Delivered = shipments.Count(c => c.Status == ShipmentStatus.Delivered),
                Failed = shipments.Count(c => c.Status == ShipmentStatus.Failed),
                TotalCount = shipments.Count()
            };

            return GeneralResponse<GetAllStatusCountDTO>.SuccessResponse("All Shipments status count retrieved successfully", data);
        }
    }
}
