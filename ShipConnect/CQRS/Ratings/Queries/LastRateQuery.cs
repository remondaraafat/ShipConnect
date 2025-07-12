using ShipConnect.DTOs.RatingDTOs;

namespace ShipConnect.CQRS.Ratings.Queries
{
    public class LastRateQuery:IRequest<GeneralResponse<ShipmentRatingDto>>
    {
        public string UserId { get;}
        public LastRateQuery(string userId)
        {
            UserId = userId;
        }
    }

    public class LastRateQueryHandler:IRequestHandler<LastRateQuery, GeneralResponse<ShipmentRatingDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        public LastRateQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<ShipmentRatingDto>> Handle(LastRateQuery request, CancellationToken cancellationToken)
        {
            var company = await unitOfWork.ShippingCompanyRepository.GetFirstOrDefaultAsync(c => c.UserId == request.UserId);
            if (company == null)
                return GeneralResponse<ShipmentRatingDto>.FailResponse("User not found");

            var lastRate =await unitOfWork.OfferRepository
                                    .GetWithFilterAsync(o => o.ShippingCompanyId == company.Id
                                                        && o.IsAccepted
                                                        && o.Ratings!=null
                                                        && o.Shipment.Status == ShipmentStatus.Delivered)
                                    .OrderByDescending(r => r.Ratings.CreatedAt)
                                    .Select(r=>new ShipmentRatingDto
                                    {
                                        ShipmentId = r.ShipmentId,
                                        ShipmentCode=r.Shipment.Code,
                                        Score = r.Ratings.Score,
                                        Comment=r.Ratings.Comment,
                                        StartUpName=r.Ratings.StartUp.CompanyName,
                                        ImageUrl =r.Ratings.StartUp.User.ProfileImageUrl,
                                        RatedAt = r.Ratings.CreatedAt,
                                    }).FirstOrDefaultAsync(cancellationToken);

            if(lastRate == null)
                return GeneralResponse<ShipmentRatingDto>.FailResponse("No ratings yet");

            return GeneralResponse<ShipmentRatingDto>
                   .SuccessResponse("Latest shipment rating fetched", lastRate);
        }
    }
}
