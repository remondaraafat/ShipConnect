using ShipConnect.CQRS.Notification.Commands;
using ShipConnect.DTOs.NotificationDTO;
using ShipConnect.DTOs.OfferDTOs;


namespace ShipConnect.CQRS.Offers.Commands
{
    public class UpdateOfferCommand : IRequest<GeneralResponse<ReadOfferDto>>
    {
        public string UserId { get;}

        public int OfferId { get;}
        public UpdateOfferDto Dto { get;}

        public UpdateOfferCommand(string userId ,int id, UpdateOfferDto dto)
        {
            UserId = userId;
            OfferId = id;
            Dto = dto;
        }
    }

    public class UpdateOfferHandler : IRequestHandler<UpdateOfferCommand, GeneralResponse<ReadOfferDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public UpdateOfferHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<GeneralResponse<ReadOfferDto>> Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.ShippingCompanyRepository.GetFirstOrDefaultAsync(c => c.UserId == request.UserId);
            if (company is null)
                return GeneralResponse<ReadOfferDto>.FailResponse("Unauthorized user");

            var offer = await _unitOfWork.OfferRepository
                                        .GetWithFilterAsync(o=>o.Id==request.OfferId
                                        &&o.ShippingCompanyId==company.Id
                                        &&!o.IsAccepted)
                                        .Include(o=>o.Shipment).ThenInclude(o=>o.Startup).FirstOrDefaultAsync(cancellationToken);

            if (offer == null)
                return GeneralResponse<ReadOfferDto>.FailResponse("Offer not found or already accepted");

            if (request.Dto.Price <= 0 || request.Dto.EstimatedDeliveryDays <= 0)
                return GeneralResponse<ReadOfferDto>.FailResponse("Invalid offer values");

            offer.Price = request.Dto.Price;
            offer.EstimatedDeliveryDays = request.Dto.EstimatedDeliveryDays;
            offer.Notes = request.Dto.Notes;

            _unitOfWork.OfferRepository.Update(offer);
            await _unitOfWork.SaveAsync();


            var notificationDto = new CreateNotificationDTO
            {
                Title = "Shipping Offer Updated",
                Message = $"A shipping company has updated their offer for your shipment #{offer.Shipment.Code}.",
                RecipientId = offer.Shipment.Startup.UserId,
                NotificationType = NotificationType.NewOffer,
            };

            await _mediator.Send(new CreateNotificationCommand(notificationDto));
            

            var dto = new ReadOfferDto
            {
                Id = offer.Id,
                Price = offer.Price,
                EstimatedDeliveryDays = offer.EstimatedDeliveryDays,
                Notes = offer.Notes,
                IsAccepted = offer.IsAccepted,
                ShipmentId = offer.ShipmentId,
                ShippingCompanyId = offer.ShippingCompanyId,
                CreatedAt= offer.UpdatedAt,
            };

            return GeneralResponse<ReadOfferDto>.SuccessResponse("Offer updated successfully", dto);
        }
    }
}
