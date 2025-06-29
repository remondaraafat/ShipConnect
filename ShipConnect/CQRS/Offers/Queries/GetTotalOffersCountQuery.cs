using MediatR;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Offers.Queries
{
    public class GetTotalOffersCountQuery : IRequest<int>
    {
    }


    public class GetTotalOffersCountHandler : IRequestHandler<GetTotalOffersCountQuery, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTotalOffersCountHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(GetTotalOffersCountQuery request, CancellationToken cancellationToken)
        {
            var allOffers = await _unitOfWork.OfferRepository.GetAllAsync();
            return allOffers.Count();
        }
    }
}
