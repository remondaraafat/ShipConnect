
namespace ShipConnect.CQRS.Notification.Queries
{
    public class GetUnreadNotificationsCountQuery:IRequest<GeneralResponse<int>>
    {
        public string UserId { get; set; }

        public GetUnreadNotificationsCountQuery(string userId)
        {
            UserId = userId;
        }
    }

    public class GetUnreadNotificationsCountQueryHandler : IRequestHandler<GetUnreadNotificationsCountQuery, GeneralResponse<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetUnreadNotificationsCountQueryHandler (IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<int>> Handle(GetUnreadNotificationsCountQuery request, CancellationToken cancellationToken)
        {
            var count = await unitOfWork.NotificationRepository.CountAsync(n => n.RecipientId == request.UserId && !n.IsRead);

            return GeneralResponse<int>.SuccessResponse("Unread Notifications Count", count);
        }
    }
}
