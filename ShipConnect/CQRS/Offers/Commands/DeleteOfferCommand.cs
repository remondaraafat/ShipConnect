using MediatR;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Offers.Commands
{
    public class DeleteOfferCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public DeleteOfferCommand(int id) => Id = id;
    }

    public class DeleteOfferHandler : IRequestHandler<DeleteOfferCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteOfferHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteOfferCommand request, CancellationToken cancellationToken)
        {
            var offer = await _unitOfWork.OfferRepository.GetByIdAsync(request.Id);
            if (offer == null) return false;

            await _unitOfWork.OfferRepository.DeleteAsync(o => o.Id == request.Id);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
