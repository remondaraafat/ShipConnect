using ShipConnect.DTOs.ShipmentDTOs;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class AdminStartUpShippingMethodCountQuery : IRequest<GeneralResponse<GetShippingMethodCountDTO>>
    {
        public int StartUpID { get;}

        public AdminStartUpShippingMethodCountQuery(int startUpId)
        {
            StartUpID = startUpId;            
        }
    }

    public class AdminStartUpShippingMethodCountQueryHandler : IRequestHandler<AdminStartUpShippingMethodCountQuery, GeneralResponse<GetShippingMethodCountDTO>>
    {
        public IUnitOfWork UnitOfWork { get; }

        public AdminStartUpShippingMethodCountQueryHandler(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<GetShippingMethodCountDTO>> Handle(AdminStartUpShippingMethodCountQuery request, CancellationToken cancellationToken)
        {
            var startUp = await UnitOfWork.StartUpRepository.GetFirstOrDefaultAsync(s => s.Id == request.StartUpID);

            if (startUp == null)
                return GeneralResponse<GetShippingMethodCountDTO>.FailResponse("Startup not found");

            var shipment = UnitOfWork.ShipmentRepository.GetWithFilterAsync(s => s.StartupId == startUp.Id);

            var grouped = await shipment.GroupBy(s => 1) // يتأكد أننا نرجع صفًّا واحدًا
                            .Select(g => new
                            {
                                TotalCount = g.Count(),
                                Land = g.Count(c => c.TransportType == TransportType.Land),
                                Sea = g.Count(c => c.TransportType == TransportType.Sea),
                                Air = g.Count(c => c.TransportType == TransportType.Air),
                            }).FirstOrDefaultAsync(cancellationToken);

            if (grouped == null || grouped.TotalCount == 0)
                return GeneralResponse<GetShippingMethodCountDTO>.FailResponse("You didn't add any shipment");

            var data = new GetShippingMethodCountDTO
            {
                TotalCount = grouped.TotalCount,
                Land = grouped.Land,
                Sea = grouped.Sea,
                Air = grouped.Air,
            };

            return GeneralResponse<GetShippingMethodCountDTO>.SuccessResponse("All Shipping Method count retrieved successfully", data);
        }
    }
}
