using MediatR;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Offers.Queries
{
    public class GetTotalOffersCountQuery : IRequest<GeneralResponse<int>>
    {
    }

    public class GetTotalOffersCountHandler : IRequestHandler<GetTotalOffersCountQuery, GeneralResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTotalOffersCountHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<int>> Handle(GetTotalOffersCountQuery request, CancellationToken cancellationToken)
        {
            var allOffers = await _unitOfWork.OfferRepository.GetAllAsync();
            int count = allOffers.Count();

            return GeneralResponse<int>.SuccessResponse("Total offers counted successfully", count);
        }
    }
}
