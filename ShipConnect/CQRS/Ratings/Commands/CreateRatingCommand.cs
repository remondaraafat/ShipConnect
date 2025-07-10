using MediatR;
using ShipConnect.CQRS.Notification.Commands;
using ShipConnect.DTOs.NotificationDTO;
using ShipConnect.DTOs.RatingDTOs;
using ShipConnect.Helpers;
using ShipConnect.Migrations;
using ShipConnect.Models;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Ratings.Commands
{
    public class CreateRatingCommand : IRequest<GeneralResponse<string>>
    {
        public string UserId { get; set; }
        public CreateRatingDto Dto { get; set; }

        public CreateRatingCommand(string userId,CreateRatingDto dto)
        {
            UserId = userId;
            Dto = dto;
        }
    }


    public class CreateRatingHandler : IRequestHandler<CreateRatingCommand, GeneralResponse<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public CreateRatingHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<GeneralResponse<string>> Handle(CreateRatingCommand request, CancellationToken cancellationToken)
        {
            var startUp = await _unitOfWork.StartUpRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserId);
            if (startUp == null)
                return GeneralResponse<string>.FailResponse("Unauthorized user");

            var offer = _unitOfWork.OfferRepository
                            .GetWithFilterAsync(o => o.Id == request.Dto.OfferId &&
                            o.IsAccepted &&
                            o.Shipment.Status == ShipmentStatus.Delivered &&
                            o.Shipment.StartupId == startUp.Id)
                            .Include(s => s.Ratings).FirstOrDefault();

            if (offer== null)
                return GeneralResponse<string>.FailResponse("Offer not found or shipment not delivered");

            if(offer.Ratings != null)
                return GeneralResponse<string>.FailResponse("You have already rated this shipment");

            var rate = new Rating
            {
                StartUpId = startUp.Id,
                Score = request.Dto.Score,
                Comment = request.Dto.Comment,
                OfferId = request.Dto.OfferId,
                CompanyId = offer.ShippingCompanyId,
            };

            var compny = await _unitOfWork.ShippingCompanyRepository.GetFirstOrDefaultAsync(s => s.Id == offer.ShippingCompanyId);
            var notificationDto = new CreateNotificationDTO
            {
                Title = "You received a new rating",
                Message = $"Your shipment has been rated by {startUp.CompanyName}",
                RecipientId = compny.UserId,
                NotificationType = NotificationType.RatingReceived,
            };

            await _mediator.Send(new CreateNotificationCommand(notificationDto));

            await _unitOfWork.RatingRepository.AddAsync(rate);
            await _unitOfWork.SaveAsync();

            return GeneralResponse<string>.SuccessResponse("Rating added successfully");
        }
    }

}
