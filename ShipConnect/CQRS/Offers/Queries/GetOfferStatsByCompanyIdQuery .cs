using MediatR;
using ShipConnect.DTOs.OfferDTOs;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Offers.Queries
{
    public class GetOfferStatsByCompanyIdQuery : IRequest<GeneralResponse<OfferStatsDto>>
    {
        public int ShippingCompanyId { get; set; }

        public GetOfferStatsByCompanyIdQuery(int shippingCompanyId)
        {
            ShippingCompanyId = shippingCompanyId;
        }
    }

    public class GetOfferStatsByCompanyIdHandler : IRequestHandler<GetOfferStatsByCompanyIdQuery, GeneralResponse<OfferStatsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOfferStatsByCompanyIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<OfferStatsDto>> Handle(GetOfferStatsByCompanyIdQuery request, CancellationToken cancellationToken)
        {
            var offers = await _unitOfWork.OfferRepository.GetWithFilterAsync(
                o => o.ShippingCompanyId == request.ShippingCompanyId
            );

            if (!offers.Any())
            {
                return GeneralResponse<OfferStatsDto>.FailResponse("No offers found for this shipping company.");
            }

            var acceptedCount = offers.Count(o => o.IsAccepted);
            var rejectedCount = offers.Count(o => !o.IsAccepted);

            var dto = new OfferStatsDto
            {
                AcceptedOffersCount = acceptedCount,
                RejectedOffersCount = rejectedCount
            };

            return GeneralResponse<OfferStatsDto>.SuccessResponse("Offer statistics retrieved", dto);
        }
    }
}
