using MediatR;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Ratings.Queries
{
    public class GetCompanyRatingAverageQuery : IRequest<double?>
    {
        public int CompanyId { get; }

        public GetCompanyRatingAverageQuery(int companyId)
        {
            CompanyId = companyId;
        }
    }


    public class GetCompanyRatingAverageHandler : IRequestHandler<GetCompanyRatingAverageQuery, double?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCompanyRatingAverageHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<double?> Handle(GetCompanyRatingAverageQuery request, CancellationToken cancellationToken)
        {
            var allRatings = await _unitOfWork.RatingRepository.GetAllAsync();
            var companyRatings = allRatings.Where(r => r.CompanyId == request.CompanyId);

            if (!companyRatings.Any())
                return null; // No ratings

            return companyRatings.Average(r => r.Score);
        }
    }
}
