using MediatR;
using ShipConnect.DTOs.OfferDTOs;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Offers.Queries
{
    public class GetOfferStatusQuery : IRequest<GeneralResponse<OfferStatsDto>>
    {
        public string UserId { get; }

        public GetOfferStatusQuery(string userId)
        {
            UserId = userId;
        }
    }

    public class GetOfferStatusQueryHandler : IRequestHandler<GetOfferStatusQuery, GeneralResponse<OfferStatsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOfferStatusQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<OfferStatsDto>> Handle(GetOfferStatusQuery request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.ShippingCompanyRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserId);
            if (company == null)
                return GeneralResponse<OfferStatsDto>.FailResponse("Unauthorized user");

            var offers = _unitOfWork.OfferRepository.GetWithFilterAsync(o => o.ShippingCompanyId == company.Id);

            int totalCount = await offers.CountAsync(cancellationToken);
            if (totalCount == 0)
                return GeneralResponse<OfferStatsDto>.FailResponse("You don't have any offers");


            var acceptedOffers = await offers.CountAsync(o => o.IsAccepted, cancellationToken);
            var rejectedOffers = totalCount - acceptedOffers;

            var dto = new OfferStatsDto
            {
                Accepted = acceptedOffers,
                Rejected = rejectedOffers,
                TotalCount = totalCount
            };

            return GeneralResponse<OfferStatsDto>.SuccessResponse("Offers status count retrieved successfully", dto);
        }
    }
}
