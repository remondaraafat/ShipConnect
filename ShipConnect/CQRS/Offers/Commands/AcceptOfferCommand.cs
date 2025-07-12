
using ShipConnect.CQRS.Notification.Commands;
using ShipConnect.DTOs.NotificationDTO;
using ShipConnect.Models;

namespace ShipConnect.CQRS.Offers.Commands
{
    public class AcceptOfferCommand:IRequest<GeneralResponse<string>>
    {
        public string UserId { get;}
        public int OfferId { get;}

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
                return GeneralResponse<string>.FailResponse("User not found");

            var offer = await _unitOfWork.OfferRepository
                                        .GetWithFilterAsync(o => o.Id == request.OfferId 
                                        && !o.IsAccepted
                                        && o.Shipment.Startup.Id==startUp.Id
                                        && o.Shipment.Status==ShipmentStatus.Pending)
                                        .Include(s=>s.Shipment)
                                        .FirstOrDefaultAsync(cancellationToken);

            if (offer == null)
                return GeneralResponse<string>.FailResponse("Offer not found or already accepted");

            var shipment = offer.Shipment!;


            //check if shipment has any accepted offer
            var checkoffers = await _unitOfWork.OfferRepository
                                            .GetFirstOrDefaultAsync(o => o.ShipmentId == offer.Shipment.Id
                                            && o.IsAccepted);

            if (checkoffers != null)
                return GeneralResponse<string>.FailResponse("This shipment already has an accepted offer");

            offer.IsAccepted = true;
            offer.Shipment.Status = ShipmentStatus.Preparing;

            //Reject other offers
            var rejectedOffers = await _unitOfWork.OfferRepository
                                                .GetWithFilterAsync(o=>o.ShipmentId==offer.ShipmentId && o.Id!=offer.Id)
                                                .ToListAsync(cancellationToken);

            foreach(var o in rejectedOffers)
                o.IsAccepted = false;

            await _unitOfWork.SaveAsync();

            var company = await _unitOfWork.ShippingCompanyRepository
                                            .GetFirstOrDefaultAsync(c => c.Id == offer.ShippingCompanyId);
            
            if (company != null)
            {
                var notification = new CreateNotificationDTO
                {
                    Title = "Offer Accepted",
                    Message = $"Great news! Your offer for shipment {shipment.Code} has been accepted by {startUp.CompanyName}. Prepare to deliver!",
                    RecipientId = company.UserId,
                    NotificationType = NotificationType.OfferAccepted,
                };

                await _mediator.Send(new CreateNotificationCommand(notification));
            }
            return GeneralResponse<string>.SuccessResponse("Offer accepted successfully");
        }
    }
}
