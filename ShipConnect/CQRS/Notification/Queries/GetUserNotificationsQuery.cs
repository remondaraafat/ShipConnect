using ShipConnect.DTOs.NotificationDTO;

namespace ShipConnect.CQRS.Notification.Queries
{
    public class GetUserNotificationsQuery:IRequest<GeneralResponse<List<NotificationDTO>>>   
    {
        public string UserId { get; set; }

        public GetUserNotificationsQuery(string userId)
        {
            UserId = userId;
        }
    }

    public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, GeneralResponse<List<NotificationDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserNotificationsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<List<NotificationDTO>>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            var notifications = _unitOfWork.NotificationRepository
                .GetWithFilterAsync(n => n.RecipientId == request.UserId && !n.IsDeleted)
                .OrderByDescending(o => o.CreatedAt)
                .Select(n => new NotificationDTO
                {
                    Title = n.Title,
                    Message = n.Message,
                    CreatedAt = n.CreatedAt,
                    IsRead = n.IsRead,
                    NotificationType = n.Type
                }).ToList();

            return GeneralResponse<List<NotificationDTO>>.SuccessResponse("Notifications retrieved successfully",notifications);
        }


    }
}
