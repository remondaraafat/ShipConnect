using MediatR;
using ShipConnect.DTOs.RatingDTOs;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Ratings.Queries
{
    public class GetRatingsByCompanyIdQuery : IRequest<GeneralResponse<List<ReadRatingDto>>>
    {
        public int CompanyId { get; set; }
        public GetRatingsByCompanyIdQuery(int companyId) => CompanyId = companyId;
    }

    public class GetRatingsByCompanyIdHandler : IRequestHandler<GetRatingsByCompanyIdQuery, GeneralResponse<List<ReadRatingDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRatingsByCompanyIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<List<ReadRatingDto>>> Handle(GetRatingsByCompanyIdQuery request, CancellationToken cancellationToken)
        {
            var ratings = await _unitOfWork.RatingRepository.GetWithFilterAsync(r => r.CompanyId == request.CompanyId);

            var dtoList = ratings.Select(r => new ReadRatingDto
            {
                Id = r.Id,
                StartUpId = r.StartUpId,
                CompanyId = r.CompanyId,
                OfferId = r.OfferId,
                Score = r.Score,
                Comment = r.Comment
            }).ToList();

            return GeneralResponse<List<ReadRatingDto>>.SuccessResponse("Ratings retrieved successfully", dtoList);
        }
    }
}

  
