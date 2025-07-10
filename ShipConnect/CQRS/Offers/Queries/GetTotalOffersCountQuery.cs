using MediatR;
using ShipConnect.DTOs.OfferDTOs;
using ShipConnect.DTOs.ShipmentDTOs;
using ShipConnect.Helpers;
using ShipConnect.Models;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Offers.Queries
{
    public class GetTotalOffersCountQuery : IRequest<GeneralResponse<OfferStatusCountDTO>>
    {
        public string UserId { get; set; }

        public GetTotalOffersCountQuery(string userId)
        {
            UserId = userId;
        }
    }

    public class GetTotalOffersCountHandler : IRequestHandler<GetTotalOffersCountQuery, GeneralResponse<OfferStatusCountDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTotalOffersCountHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<OfferStatusCountDTO>> Handle(GetTotalOffersCountQuery request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.ShippingCompanyRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserId);
            if (company == null)
                return GeneralResponse<OfferStatusCountDTO>.FailResponse("Unauthorized user");

            var offers = _unitOfWork.OfferRepository.GetWithFilterAsync(o => o.ShippingCompanyId == company.Id).ToList();

            int totalCount = offers.Count();
            if (totalCount == 0)
                return GeneralResponse<OfferStatusCountDTO>.FailResponse("You don't have any offer");

            var data = new OfferStatusCountDTO
            {
                Accepted = offers.Count(o => o.IsAccepted),
                Rejected = offers.Count(o=>!o.IsAccepted),
                TotalCount = totalCount
            };

            return GeneralResponse<OfferStatusCountDTO>.SuccessResponse("Total offers counted successfully", data);
        }
    }
}
