using MediatR;
using ShipConnect.DTOs.OfferDTOs;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Offers.Queries
{
    public class GetOfferStatsByCompanyIdQuery : IRequest<OfferStatsDto>
    {
        public int ShippingCompanyId { get; set; }

        public GetOfferStatsByCompanyIdQuery(int shippingCompanyId)
        {
            ShippingCompanyId = shippingCompanyId;
        }
    }

    public class GetOfferStatsByCompanyIdHandler : IRequestHandler<GetOfferStatsByCompanyIdQuery, OfferStatsDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOfferStatsByCompanyIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OfferStatsDto> Handle(GetOfferStatsByCompanyIdQuery request, CancellationToken cancellationToken)
        {
            var offers = await _unitOfWork.OfferRepository.GetWithFilterAsync(
                o => o.ShippingCompanyId == request.ShippingCompanyId
            );

            var acceptedCount = offers.Count(o => o.IsAccepted);
            var rejectedCount = offers.Count(o => !o.IsAccepted);

            return new OfferStatsDto
            {
                AcceptedOffersCount = acceptedCount,
                RejectedOffersCount = rejectedCount
            };
        }
    }
}
