using ShipConnect.DTOs.ShipmentDTOs;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class AdminStartUpShippingScopeCountQuery : IRequest<GeneralResponse<GetShippingScopeCountDTO>>
    {
        public int StartUpID { get;}

        public AdminStartUpShippingScopeCountQuery(int startUpId)
        {
            StartUpID= startUpId;            
        }
    }

    public class AdminStartUpShippingScopeCountQueryHandler : IRequestHandler<AdminStartUpShippingScopeCountQuery, GeneralResponse<GetShippingScopeCountDTO>>
    {
        public IUnitOfWork UnitOfWork { get; }

        public AdminStartUpShippingScopeCountQueryHandler(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<GetShippingScopeCountDTO>> Handle(AdminStartUpShippingScopeCountQuery request, CancellationToken cancellationToken)
        {
            var startUp = await UnitOfWork.StartUpRepository.GetFirstOrDefaultAsync(s => s.Id == request.StartUpID);

            if (startUp == null)
                return GeneralResponse<GetShippingScopeCountDTO>.FailResponse("Startup not found");

            var shipment = UnitOfWork.ShipmentRepository.GetWithFilterAsync(s => s.StartupId == startUp.Id);

            var grouped = await shipment.GroupBy(s => 1) // يتأكد أننا نرجع صفًّا واحدًا
                            .Select(g => new
                            {
                                TotalCount = g.Count(),
                                Domestic = g.Count(c => c.ShippingScope == ShippingScope.Domestic),
                                International = g.Count(c => c.ShippingScope == ShippingScope.International),
                            }).FirstOrDefaultAsync(cancellationToken);

            if (grouped == null || grouped.TotalCount == 0)
                return GeneralResponse<GetShippingScopeCountDTO>.FailResponse("You didn't add any shipment");

            var data = new GetShippingScopeCountDTO
            {
                TotalCount = grouped.TotalCount,
                Domestic = grouped.Domestic,
                International = grouped.International,
            };

            return GeneralResponse<GetShippingScopeCountDTO>.SuccessResponse("All Shipping Method count retrieved successfully", data);
        }
    }
}
