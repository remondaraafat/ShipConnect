using MediatR;
using ShipConnect.DTOs.RatingDTOs;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Ratings.Commands
{
    public class UpdateRatingCommand : IRequest<ReadRatingDto?>
    {
        public int Id { get; set; }
        public UpdateRatingDto Dto { get; set; }
        public UpdateRatingCommand(int id, UpdateRatingDto dto)
        {
            Id = id;
            Dto = dto;
        }
    }

    public class UpdateRatingHandler : IRequestHandler<UpdateRatingCommand, ReadRatingDto?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRatingHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ReadRatingDto?> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
        {
            var rating = await _unitOfWork.RatingRepository.GetByIdAsync(request.Id);
            if (rating == null) return null;

            rating.Score = request.Dto.Score;
            rating.Comment = request.Dto.Comment;
            rating.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.RatingRepository.Update(rating);
            await _unitOfWork.SaveAsync();

            return new ReadRatingDto
            {
                Id = rating.Id,
                StartUpId = rating.StartUpId,
                CompanyId = rating.CompanyId,
                OfferId = rating.OfferId,
                Score = rating.Score,
                Comment = rating.Comment
            };
        }
    }
}
