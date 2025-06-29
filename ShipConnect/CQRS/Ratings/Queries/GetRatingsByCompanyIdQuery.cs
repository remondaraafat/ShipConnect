using MediatR;
using ShipConnect.DTOs.RatingDTOs;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Ratings.Queries
{
    public class GetRatingsByCompanyIdQuery : IRequest<List<ReadRatingDto>>
    {
        public int CompanyId { get; set; }
        public GetRatingsByCompanyIdQuery(int companyId) => CompanyId = companyId;
    }


    public class GetRatingsByCompanyIdHandler : IRequestHandler<GetRatingsByCompanyIdQuery, List<ReadRatingDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRatingsByCompanyIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ReadRatingDto>> Handle(GetRatingsByCompanyIdQuery request, CancellationToken cancellationToken)
        {
            var ratings = await _unitOfWork.RatingRepository
                .GetWithFilterAsync(r => r.CompanyId == request.CompanyId);

            return ratings.Select(r => new ReadRatingDto
            {
                Id = r.Id,
                StartUpId = r.StartUpId,
                CompanyId = r.CompanyId,
                OfferId = r.OfferId,
                Score = r.Score,
                Comment = r.Comment
            }).ToList();
        }

    }

}
