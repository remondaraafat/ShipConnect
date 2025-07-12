using ShipConnect.DTOs.ShipmentDTOs;

namespace ShipConnect.CQRS.shipments.Queries
{
    public class AdminStatusCountQuery : IRequest<GeneralResponse<GetAllStatusCountDTO>>
    {
    }

    public class AdminStatusCountQueryHandler : IRequestHandler<AdminStatusCountQuery, GeneralResponse<GetAllStatusCountDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminStatusCountQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<GetAllStatusCountDTO>> Handle(AdminStatusCountQuery request, CancellationToken cancellationToken)
        {
            var shipments = _unitOfWork.ShipmentRepository.GetAllAsync();

            //بعمل كدة عشان الاستعلام يتنفذ مرة واحدة بس ف الداتا بيز
            var grouped = await shipments.GroupBy(s => 1) // يتأكد أننا نرجع صفًّا واحدًا
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

