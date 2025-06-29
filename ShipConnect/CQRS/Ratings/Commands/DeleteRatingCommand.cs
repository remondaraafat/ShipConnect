using MediatR;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Ratings.Commands
{
    public class DeleteRatingCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public DeleteRatingCommand(int id) => Id = id;
    }

    public class DeleteRatingHandler : IRequestHandler<DeleteRatingCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRatingHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteRatingCommand request, CancellationToken cancellationToken)
        {
            var rating = await _unitOfWork.RatingRepository.GetByIdAsync(request.Id);
            if (rating == null) return false;

            await _unitOfWork.RatingRepository.DeleteAsync(r => r.Id == request.Id);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }

}
