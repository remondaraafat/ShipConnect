
using ShipConnect.CQRS.Notification.Commands;
using ShipConnect.DTOs.NotificationDTO;
using ShipConnect.Models;

namespace ShipConnect.CQRS.Offers.Commands
{
    public class AcceptOfferCommand:IRequest<GeneralResponse<string>>
    {
        public string UserId { get; set; }
        public int OfferId { get; set; }

        public AcceptOfferCommand(string userId, int offerId)
        {
            UserId = userId;
            OfferId = offerId;
        }
    }


    public class AcceptOfferCommandHandler:IRequestHandler<AcceptOfferCommand, GeneralResponse<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public AcceptOfferCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;      
        }

        public async Task<GeneralResponse<string>> Handle(AcceptOfferCommand request, CancellationToken cancellationToken)
        {
            var startUp = await _unitOfWork.StartUpRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserId);
            if (startUp is null)
                return GeneralResponse<string>.FailResponse("Unauthorized user");

            var offer = await _unitOfWork.OfferRepository.GetFirstOrDefaultAsync(o => o.Id == request.OfferId);
            if (offer == null)
                return GeneralResponse<string>.FailResponse("Offer not found");

            var shipment = await _unitOfWork.ShipmentRepository
                .GetFirstOrDefaultAsync(s => s.Id == offer.ShipmentId && s.StartupId == startUp.Id);
            if (shipment == null)
                return GeneralResponse<string>.FailResponse("Shipment not found");

            //check if shipment has any accepted offer
            var checkoffers = await _unitOfWork.OfferRepository.GetFirstOrDefaultAsync(o => o.ShipmentId == shipment.Id && o.IsAccepted);
            if (checkoffers != null)
                return GeneralResponse<string>.FailResponse("This shipment already has an accepted offer");

            //Accept offer and update shipment status
            offer.IsAccepted = true;
            shipment.Status = ShipmentStatus.Preparing;

            _unitOfWork.OfferRepository.Update(offer);
            _unitOfWork.ShipmentRepository.Update(shipment);

            //Reject other offers
            var rejectedOffers = _unitOfWork.OfferRepository.GetWithFilterAsync(o=>o.ShipmentId==offer.ShipmentId && o.Id!=offer.Id).ToList();

            foreach(var o in rejectedOffers)
            {
                o.IsAccepted = false;
                _unitOfWork.OfferRepository.Update(o);
            }

            await _unitOfWork.SaveAsync();

            
            //send notification to shipping company
            var company = await _unitOfWork.ShippingCompanyRepository.GetFirstOrDefaultAsync(c => c.Id == offer.ShippingCompanyId);
            if (company == null)
                return GeneralResponse<string>.FailResponse("Shipping company not found");

            var notification = new CreateNotificationDTO
            {
                Title = "Offer Accepted",
                Message = $"Great news! Your offer for shipment {shipment.Code} has been accepted by {startUp.CompanyName}. Prepare to deliver!",
                RecipientId = company.UserId,
                NotificationType = NotificationType.OfferAccepted,
            };

            await _mediator.Send(new CreateNotificationCommand(notification));

            return GeneralResponse<string>.SuccessResponse("Offer accepted successfully");
        }
    }
}
