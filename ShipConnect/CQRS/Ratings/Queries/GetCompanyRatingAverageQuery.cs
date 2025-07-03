using MediatR;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Ratings.Queries
{
    public class GetCompanyRatingAverageQuery : IRequest<GeneralResponse<double?>>
    {
        public int CompanyId { get; }
        public GetCompanyRatingAverageQuery(int companyId) => CompanyId = companyId;
    }


    public class GetCompanyRatingAverageHandler : IRequestHandler<GetCompanyRatingAverageQuery, GeneralResponse<double?>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCompanyRatingAverageHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<double?>> Handle(GetCompanyRatingAverageQuery request, CancellationToken cancellationToken)
        {
            
            var companyRatings = _unitOfWork.RatingRepository.GetAllAsync().Where(r => r.CompanyId == request.CompanyId);

            if (!companyRatings.Any())
                return GeneralResponse<double?>.FailResponse("No ratings found for this company");

            var average = companyRatings.Average(r => r.Score);
            return GeneralResponse<double?>.SuccessResponse("Average rating retrieved", average);
        }
    }



}
