using MediatR;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Offers.Commands
{
    public class UpdateOfferStatusCommand : IRequest<bool>
    {
        public int OfferId { get; set; }
        public bool IsAccepted { get; set; }

        public UpdateOfferStatusCommand(int offerId, bool isAccepted)
        {
            OfferId = offerId;
            IsAccepted = isAccepted;
        }
    }

    public class UpdateOfferStatusHandler : IRequestHandler<UpdateOfferStatusCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOfferStatusHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateOfferStatusCommand request, CancellationToken cancellationToken)
        {
            var offer = await _unitOfWork.OfferRepository.GetByIdAsync(request.OfferId);
            if (offer == null) return false;

            offer.IsAccepted = request.IsAccepted;
            _unitOfWork.OfferRepository.Update(offer);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
