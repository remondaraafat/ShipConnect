using MediatR;
using ShipConnect.DTOs.OfferDTOs;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Offers.Queries
{
    public class GetOfferStatusQuery : IRequest<GeneralResponse<OfferStatsDto>>
    {
        public int? CompanyId { get; }
        public string? UserId { get; }

        public GetOfferStatusQuery(int? companyId, string? userId)
        {
            CompanyId = companyId;
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
            var offersQry = _unitOfWork.OfferRepository.GetAllAsync();

            if (request.CompanyId != null)
                offersQry = offersQry.Where(s => s.ShippingCompanyId == request.CompanyId);

            else if (!string.IsNullOrEmpty(request.UserId))
                offersQry = offersQry.Where(s => s.ShippingCompany.UserId == request.UserId);

            else
                return GeneralResponse<OfferStatsDto>.FailResponse("No identifier provided");
            
            var grouped = await offersQry.GroupBy(_ => 1)    // صفّ واحد
                                         .Select(g => new
                                         {
                                             Total = g.Count(),
                                             Accepted = g.Count(x => x.IsAccepted)
                                         })
                                         .FirstOrDefaultAsync(cancellationToken);

            if (grouped is null || grouped.Total == 0)
                return GeneralResponse<OfferStatsDto>.FailResponse("No offers found");

            var dto = new OfferStatsDto
            {
                TotalCount = grouped.Total,
                Accepted = grouped.Accepted,
                Rejected = grouped.Total - grouped.Accepted
            };

            return GeneralResponse<OfferStatsDto>.SuccessResponse("Offers status count retrieved successfully", dto);
        }
    }
}
