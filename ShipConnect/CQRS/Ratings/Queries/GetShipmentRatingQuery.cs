using MediatR;
using ShipConnect.DTOs.RatingDTOs;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Ratings.Queries
{
    public class GetShipmentRatingQuery : IRequest<GeneralResponse<ShipmentRatingDto>>
    {
        public string UserId { get;}
        public int ShipmentId { get;}
        public GetShipmentRatingQuery(string userId, int shipmentId)
        {
            UserId = userId;
            ShipmentId = shipmentId;
        }
    }

    public class GetShipmentRatingQueryHandler : IRequestHandler<GetShipmentRatingQuery, GeneralResponse<ShipmentRatingDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetShipmentRatingQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<ShipmentRatingDto>> Handle(GetShipmentRatingQuery request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.ShippingCompanyRepository.GetFirstOrDefaultAsync(c => c.UserId == request.UserId);
            if (company == null)
                return GeneralResponse<ShipmentRatingDto>.FailResponse("User not found");

            var rate = await _unitOfWork.OfferRepository
                                        .GetWithFilterAsync(o => o.ShippingCompanyId == company.Id
                                                            && o.ShipmentId == request.ShipmentId
                                                            && o.IsAccepted
                                                            && o.Shipment.Status == ShipmentStatus.Delivered
                                                            && o.Ratings != null)
                                        .Select(r=>new ShipmentRatingDto
                                        {
                                            ShipmentId=r.ShipmentId,
                                            ShipmentCode = r.Shipment.Code,
                                            Score = r.Ratings.Score,
                                            Comment = r.Ratings.Comment,
                                            StartUpName = r.Ratings.StartUp.CompanyName,
                                            ImageUrl = r.Ratings.StartUp.User.ProfileImageUrl,
                                            RatedAt = r.Ratings.CreatedAt
                                        }).FirstOrDefaultAsync(cancellationToken);

            if (rate == null)
                return GeneralResponse<ShipmentRatingDto>.FailResponse("Shipment not rated yet");

            return GeneralResponse<ShipmentRatingDto>.SuccessResponse("Ratings retrieved successfully", rate);
        }
    }
}


