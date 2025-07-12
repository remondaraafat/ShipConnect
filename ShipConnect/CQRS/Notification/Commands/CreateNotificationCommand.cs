using Microsoft.AspNetCore.SignalR;
using ShipConnect.DTOs.NotificationDTO;
using ShipConnect.Hubs;
using ShipConnect.Models;

//using MediatR;
//using ShipConnect.CQRS.Notification.Commands;
//using ShipConnect.Wrappers;
namespace ShipConnect.CQRS.Notification.Commands
{
    public class CreateNotificationCommand:IRequest<GeneralResponse<string>>
    {
        public CreateNotificationDTO NotificationDTO { get; set; }

        public CreateNotificationCommand(CreateNotificationDTO dto)
        {
            NotificationDTO = dto;
        }
    }

    public class CreateNotificationCommandHandler:IRequestHandler<CreateNotificationCommand, GeneralResponse<string>>   
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationHub> hubContext;

        public CreateNotificationCommandHandler(IUnitOfWork unitOfWork, IHubContext<NotificationHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            this.hubContext = hubContext;
        }

        public async Task<GeneralResponse<string>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var dto=request.NotificationDTO;

            var notification = new ShipConnect.Models.Notification
            {
                Title = dto.Title,
                Message = dto.Message,
                RecipientId = dto.RecipientId,
                Type = dto.NotificationType,
                CreatedAt = DateTime.Now,
            };

            await _unitOfWork.NotificationRepository.AddAsync(notification);
            await _unitOfWork.SaveAsync();

            await hubContext.Clients.User(dto.RecipientId).SendAsync("ReceiveNotification", new
            {
                notification.Title,
                notification.Message,
                notification.Type,
                notification.CreatedAt,
            });

            return GeneralResponse<string>.SuccessResponse("Notification sent and saved successfully");
        }
    }
}
