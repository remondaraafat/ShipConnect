using ShipConnect.DTOs.ShipmentDTOs;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class GetMonthlyDeliveryPerformanceQuery:IRequest<GeneralResponse<List<MonthlyDeliveryDto>>>
    {
        public string UserId { get; set; }

        public GetMonthlyDeliveryPerformanceQuery(string userId) => UserId = userId;
    }

    public class GetMonthlyDeliveryPerformanceQueryHandler : IRequestHandler<GetMonthlyDeliveryPerformanceQuery, GeneralResponse<List<MonthlyDeliveryDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMonthlyDeliveryPerformanceQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;            
        }

        public async Task<GeneralResponse<List<MonthlyDeliveryDto>>> Handle(GetMonthlyDeliveryPerformanceQuery request, CancellationToken cancellationToken)
        {
            var startUp = await _unitOfWork.StartUpRepository
                .GetFirstOrDefaultAsync(s => s.UserId == request.UserId);

            if (startUp == null)
                return GeneralResponse<List<MonthlyDeliveryDto>>.FailResponse("Unauthorized user");

            var shipment = _unitOfWork.OfferRepository
                                    .GetWithFilterAsync(o => o.IsAccepted &&
                                    o.Shipment!.StartupId == startUp.Id &&
                                    o.Shipment.Status == ShipmentStatus.Delivered)
                                    .Select(s => new
                                    {
                                        s.Shipment,
                                        s.Shipment.ActualDelivery,
                                        ExpectedDeliveryDate = s.Shipment.RequestedPickupDate.AddDays(s.EstimatedDeliveryDays),
                                        Month = s.Shipment.ActualDelivery!.Value.Month
                                    }).ToList();

            var grouped = shipment
                .GroupBy(s => s.Month)
                .Select(g => new MonthlyDeliveryDto
                {
                    Month = g.Key,
                    OnTime = g.Count(s => s.ActualDelivery <= s.ExpectedDeliveryDate),
                    Late = g.Count(s => s.ActualDelivery > s.ExpectedDeliveryDate)
                })
                .ToList();

            var fullYear = Enumerable.Range(1, 12).Select(m =>
            {
                var existing = grouped.FirstOrDefault(x => x.Month == m);
                return existing ?? new MonthlyDeliveryDto { Month = m, OnTime = 0, Late = 0 };
            }).OrderBy(x => x.Month).ToList();

            return GeneralResponse<List<MonthlyDeliveryDto>>.SuccessResponse("Monthly delivery stats fetched", fullYear);
        }
    }
}
