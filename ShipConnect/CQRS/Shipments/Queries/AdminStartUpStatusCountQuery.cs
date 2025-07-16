using ShipConnect.DTOs.ShipmentDTOs;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class AdminStartUpStatusCountQuery : IRequest<GeneralResponse<GetAllStatusCountDTO>>
    {
        public int StartUpID { get;}
        public AdminStartUpStatusCountQuery(int startupId)
        {
            StartUpID = startupId;
        }
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
            
            var grouped = await shipment.GroupBy(s => 1) // يتأكد أننا نرجع صفًّا واحدًا
                            .Select(g => new
                            {
                                Total = g.Count(),
                                Pending = g.Count(s => s.Status == ShipmentStatus.Pending),
                                Preparing = g.Count(s => s.Status == ShipmentStatus.Preparing),
                                InTransit = g.Count(s => s.Status == ShipmentStatus.InTransit),
                                AtWarehouse = g.Count(s => s.Status == ShipmentStatus.AtWarehouse),
                                OutForDelivery = g.Count(s => s.Status == ShipmentStatus.OutForDelivery),
                                Delivered = g.Count(s => s.Status == ShipmentStatus.Delivered),
                                Failed = g.Count(s => s.Status == ShipmentStatus.Failed)
                            }).FirstOrDefaultAsync(cancellationToken);

            if (grouped == null || grouped.Total == 0)
                return GeneralResponse<GetAllStatusCountDTO>.FailResponse("No Shipments yet");

            var data = new GetAllStatusCountDTO
            {
                TotalCount = grouped.Total,
                Pending = grouped.Pending,
                Preparing = grouped.Preparing,
                InTransit = grouped.InTransit,
                AtWarehouse = grouped.AtWarehouse,
                OutForDelivery = grouped.OutForDelivery,
                Delivered = grouped.Delivered,
                Failed = grouped.Failed
            };

            return GeneralResponse<GetAllStatusCountDTO>.SuccessResponse("All Shipments status count retrieved successfully", data);
        }
    }

}
