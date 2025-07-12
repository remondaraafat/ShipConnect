using ShipConnect.DTOs.OfferDTOs;

namespace ShipConnect.CQRS.Offers.Queries
{
    public class AdminOfferStatsCountQuery : IRequest<GeneralResponse<OfferStatsDto>>
    {
    }

    public class AdminOfferStatsCountQueryHandler : IRequestHandler<AdminOfferStatsCountQuery, GeneralResponse<OfferStatsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminOfferStatsCountQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<OfferStatsDto>> Handle(AdminOfferStatsCountQuery request, CancellationToken cancellationToken)
        {
            var totalCount = await _unitOfWork.OfferRepository.CountAsync();
            if (totalCount == 0)
                return GeneralResponse<OfferStatsDto>.FailResponse("No offers yet");

            var acceptedOffers = await _unitOfWork.OfferRepository.CountAsync(o=>o.IsAccepted);
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
