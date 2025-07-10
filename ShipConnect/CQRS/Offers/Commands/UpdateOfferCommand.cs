using MediatR;
using ShipConnect.CQRS.Notification.Commands;
using ShipConnect.DTOs.NotificationDTO;
using ShipConnect.DTOs.OfferDTOs;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Offers.Commands
{
    public class UpdateOfferCommand : IRequest<GeneralResponse<ReadOfferDto>>
    {
        public string UserId { get; set; }

        public int OfferId { get; set; }
        public UpdateOfferDto Dto { get; set; }

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

            var offer = await _unitOfWork.OfferRepository.GetByIdAsync(request.OfferId);
            if (offer == null)
                return GeneralResponse<ReadOfferDto>.FailResponse("Offer not found");
            
            if(offer.IsAccepted)
                return GeneralResponse<ReadOfferDto>.FailResponse("you can't update accepted offer");

            offer.Price = request.Dto.Price;
            offer.EstimatedDeliveryDays = request.Dto.EstimatedDeliveryDays;
            offer.Notes = request.Dto.Notes;

            _unitOfWork.OfferRepository.Update(offer);
            await _unitOfWork.SaveAsync();

            //send notification to startup
            var shipment = _unitOfWork.OfferRepository.GetWithFilterAsync(o => o.Id==request.OfferId).Select(s => new { s.Shipment.Startup, s.Shipment.Code }).FirstOrDefault();

            if (shipment?.Startup?.UserId != null)
            {
                var notificationDto = new CreateNotificationDTO
                {
                    Title = "Shipping Offer Updated",
                    Message = $"A shipping company has updated their offer for your shipment #{shipment.Code}.",
                    RecipientId = shipment.Startup.UserId,
                    NotificationType = NotificationType.NewOffer,
                };

                await _mediator.Send(new CreateNotificationCommand(notificationDto));
            }

            var dto = new ReadOfferDto
            {
                Id = offer.Id,
                Price = offer.Price,
                EstimatedDeliveryDays = offer.EstimatedDeliveryDays,
                Notes = offer.Notes,
                IsAccepted = offer.IsAccepted,
                ShipmentId = offer.ShipmentId,
                ShippingCompanyId = offer.ShippingCompanyId
            };

            return GeneralResponse<ReadOfferDto>.SuccessResponse("Offer updated successfully", dto);
        }
    }
}
