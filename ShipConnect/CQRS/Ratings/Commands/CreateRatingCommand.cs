using MediatR;
using ShipConnect.DTOs.RatingDTOs;
using ShipConnect.Helpers;
using ShipConnect.Models;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Ratings.Commands
{
    public class CreateRatingCommand : IRequest<GeneralResponse<ReadRatingDto>>
    {
        public CreateRatingDto Dto { get; }

        public CreateRatingCommand(CreateRatingDto dto)
        {
            Dto = dto;
        }
    }


    public class CreateRatingHandler : IRequestHandler<CreateRatingCommand, GeneralResponse<ReadRatingDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateRatingHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<ReadRatingDto>> Handle(CreateRatingCommand request, CancellationToken cancellationToken)
        {
            var entity = new Rating
            {
                StartUpId = request.Dto.StartUpId,
                CompanyId = request.Dto.CompanyId,
                OfferId = request.Dto.OfferId,
                Score = request.Dto.Score,
                Comment = request.Dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.RatingRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();

            var dto = new ReadRatingDto
            {
                Id = entity.Id,
                StartUpId = entity.StartUpId,
                CompanyId = entity.CompanyId,
                OfferId = entity.OfferId,
                Score = entity.Score,
                Comment = entity.Comment
            };

            return GeneralResponse<ReadRatingDto>.SuccessResponse("Rating created successfully", dto);
        }
    }

}
