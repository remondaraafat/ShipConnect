using ShipConnect.DTOs.OfferDTOs;

namespace ShipConnect.CQRS.Offers.Queries
{
    public class GetShipmentWithOffersQuery:IRequest<GeneralResponse<List<ShipmentWithOffersDTO>>>
    {
        public string UserId { get; }

        public GetShipmentWithOffersQuery(string userId)
        {
            this.UserId = userId;
        }
    }

    public class GetShipmentWithOffersQueryHandler :IRequestHandler<GetShipmentWithOffersQuery, GeneralResponse<List<ShipmentWithOffersDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetShipmentWithOffersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;             
        }

        public async Task<GeneralResponse<List<ShipmentWithOffersDTO>>> Handle(GetShipmentWithOffersQuery request, CancellationToken cancellationToken)
        {
            var startUp = await _unitOfWork.StartUpRepository.GetFirstOrDefaultAsync(s=>s.UserId== request.UserId);
            if (startUp == null)
                return GeneralResponse<List<ShipmentWithOffersDTO>>.FailResponse("Unauthorized user");

            var shipments = _unitOfWork.ShipmentRepository
                                    .GetWithFilterAsync(s=>s.StartupId==startUp.Id && !s.Offers.Any(o=>o.IsAccepted))
                                    .Include(s=>s.Offers)
                                    .ThenInclude(s=>s.ShippingCompany)
                                    .OrderByDescending(s=>s.CreatedAt)
                                    .ToList();

            var ratingDict = ( _unitOfWork.RatingRepository
                                    .GetAllAsync()
                                    .ToList())     
                             .GroupBy(r => r.ShippingCompany.Id)       
                             .ToDictionary(
                                 g => g.Key,
                                 g => g.Average(r => r.Score));

            var result = shipments.Select(s => new ShipmentWithOffersDTO
            {
                ShipmentCode = s.Code,
                Offers = s.Offers.OrderByDescending(o=>o.CreatedAt).Select(o => new ShipmentOffers
                {
                    OfferId = o.Id,
                    Price = o.Price,
                    EstimatedDeliveryDays = o.EstimatedDeliveryDays,
                    Notes = o.Notes,
                    CompanyRating = ratingDict.TryGetValue(o.ShippingCompany.Id, out var avg) // ← نفس المفتاح
                                              ? avg
                                              : 0
                }).ToList()
            }).ToList();

            return GeneralResponse<List<ShipmentWithOffersDTO>>.SuccessResponse("Shipment with offers retrieved successfuly",result); ;

        }
    }
}
