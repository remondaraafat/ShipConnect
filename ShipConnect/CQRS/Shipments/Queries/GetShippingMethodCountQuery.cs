using ShipConnect.DTOs.ShipmentDTOs;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class GetShippingMethodCountQuery : IRequest<GeneralResponse<GetShippingMethodCountDTO>>
    {
        public string UserId { get;}

        public GetShippingMethodCountQuery(string userId) => UserId = userId;
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
                return GeneralResponse<GetShippingMethodCountDTO>.FailResponse("User not found");

            var shipment = UnitOfWork.ShipmentRepository.GetWithFilterAsync(s => s.StartupId == startUp.Id);

            var grouped = await shipment.GroupBy(s => 1) // يتأكد أننا نرجع صفًّا واحدًا
                            .Select(g => new
                            {
                                TotalCount = g.Count(),
                                Land = g.Count(c => c.TransportType == TransportType.Land),
                                Sea = g.Count(c => c.TransportType == TransportType.Sea),
                                Air = g.Count(c => c.TransportType == TransportType.Air),
                            }).FirstOrDefaultAsync(cancellationToken);

            if (grouped == null || grouped.TotalCount==0)
                return GeneralResponse<GetShippingMethodCountDTO>.FailResponse("You didn't add any shipment");

            var data = new GetShippingMethodCountDTO
            {
                TotalCount = grouped.TotalCount,
                Land = grouped.Land,
                Sea = grouped.Sea,
                Air = grouped.Air,
            };

            return GeneralResponse<GetShippingMethodCountDTO>.SuccessResponse("Shipping method counts retrieved successfully.", data);
        }
    }
}
