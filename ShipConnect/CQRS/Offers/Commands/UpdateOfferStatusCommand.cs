using MediatR;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Offers.Commands
{
    public class UpdateOfferStatusCommand : IRequest<GeneralResponse<bool>>
    {
        public int OfferId { get; set; }
        public bool IsAccepted { get; set; }

        public UpdateOfferStatusCommand(int offerId, bool isAccepted)
        {
            OfferId = offerId;
            IsAccepted = isAccepted;
        }
    }

    public class UpdateOfferStatusHandler : IRequestHandler<UpdateOfferStatusCommand, GeneralResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOfferStatusHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<bool>> Handle(UpdateOfferStatusCommand request, CancellationToken cancellationToken)
        {
            var offer = await _unitOfWork.OfferRepository.GetByIdAsync(request.OfferId);
            if (offer == null)
                return GeneralResponse<bool>.FailResponse("Offer not found");

            offer.IsAccepted = request.IsAccepted;
            _unitOfWork.OfferRepository.Update(offer);
            await _unitOfWork.SaveAsync();

            string msg = request.IsAccepted ? "Offer accepted successfully" : "Offer rejected successfully";

            return GeneralResponse<bool>.SuccessResponse(msg, true);
        }
    }
}
