using MediatR;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Ratings.Commands
{
    public class DeleteRatingCommand : IRequest<GeneralResponse<string>>
    {
        public int Id { get; set; }
        public DeleteRatingCommand(int id) => Id = id;
    }

    public class DeleteRatingHandler : IRequestHandler<DeleteRatingCommand, GeneralResponse<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRatingHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<string>> Handle(DeleteRatingCommand request, CancellationToken cancellationToken)
        {
            var rating = await _unitOfWork.RatingRepository.GetByIdAsync(request.Id);
            if (rating == null)
                return GeneralResponse<string>.FailResponse("Rating not found");

            await _unitOfWork.RatingRepository.DeleteAsync(r => r.Id == request.Id);
            await _unitOfWork.SaveAsync();

            return GeneralResponse<string>.SuccessResponse("Rating deleted successfully");
        }
    }



}
