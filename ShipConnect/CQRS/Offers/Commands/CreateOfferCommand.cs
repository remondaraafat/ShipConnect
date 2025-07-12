using MediatR;
using Microsoft.AspNetCore.SignalR;
using ShipConnect.CQRS.Notification.Commands;
using ShipConnect.DTOs.NotificationDTO;
using ShipConnect.DTOs.OfferDTOs;
using ShipConnect.Helpers;
using ShipConnect.Hubs;
using ShipConnect.Models;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Offers.Commands
{
    // command
    public class CreateOfferCommand : IRequest<GeneralResponse<ReadOfferDto>>
    {
        public string UserId { get;}
        public CreateOfferDto Dto { get; }

        public CreateOfferCommand(string userId ,CreateOfferDto dto)
        {
            UserId = userId;
            Dto = dto;
        }
    }

    // handler
    public class CreateOfferHandler : IRequestHandler<CreateOfferCommand, GeneralResponse<ReadOfferDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public CreateOfferHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<GeneralResponse<ReadOfferDto>> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
        {

            var company = await _unitOfWork.ShippingCompanyRepository.GetFirstOrDefaultAsync(c => c.UserId == request.UserId);
            if (company is null)
                return GeneralResponse<ReadOfferDto>.FailResponse("User not found");

            var shipment = await _unitOfWork.ShipmentRepository
                                            .GetWithFilterAsync(s => s.Id == request.Dto.ShipmentId 
                                            && s.Status==ShipmentStatus.Pending
                                            && !s.Offers.Any(o=>o.IsAccepted)).Include(s => s.Startup )
                                            .FirstOrDefaultAsync(cancellationToken);

            if (shipment == null)
                return GeneralResponse<ReadOfferDto>.FailResponse("Shipment not available");

            var exists = await _unitOfWork.OfferRepository
                                            .GetFirstOrDefaultAsync(o=>o.ShippingCompanyId ==company.Id
                                            &&o.ShipmentId==request.Dto.ShipmentId
                                            &&o.Price==request.Dto.Price
                                            &&o.EstimatedDeliveryDays==request.Dto.EstimatedDeliveryDays);

            if(exists!=null)
                return GeneralResponse<ReadOfferDto>.FailResponse("Offer already submitted");

            if (request.Dto.Price <= 0 || request.Dto.EstimatedDeliveryDays <= 0)
                return GeneralResponse<ReadOfferDto>.FailResponse("Invalid offer values");

            var offer = new Offer
                {
                    Price = request.Dto.Price,
                    EstimatedDeliveryDays = request.Dto.EstimatedDeliveryDays,
                    Notes = request.Dto.Notes,
                    ShipmentId = request.Dto.ShipmentId,
                    ShippingCompanyId = company.Id,
                    IsAccepted = false,
                };

            await _unitOfWork.OfferRepository.AddAsync(offer);
            await _unitOfWork.SaveAsync();

            //send notification to startup

            if(shipment?.Startup?.UserId != null)
            {
                var notificationDto = new CreateNotificationDTO
                {
                    Title = "New Offer Received",
                    Message = $"You have received a new shipping offer for your shipment {shipment.Code} from {company.CompanyName}",
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
                CreatedAt =offer.CreatedAt,
                ShippingCompanyId=company.Id,   
            };

            return GeneralResponse<ReadOfferDto>.SuccessResponse("Offer created successfully", dto);

        }
    }
}
