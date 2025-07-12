
namespace ShipConnect.CQRS.Notification.Commands
{
    public class MarkNotificationAsReadCommand:IRequest<GeneralResponse<string>>
    {
        public string UserID { get; set; }
        public int NotificationID { get; set; }

        public MarkNotificationAsReadCommand(string UserID, int NotificationID)
        {
            this.UserID = UserID;
            this.NotificationID = NotificationID;
        }
    }

    public class MarkNotificationAsReadCommandHandler:IRequestHandler<MarkNotificationAsReadCommand,GeneralResponse<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public MarkNotificationAsReadCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<string>> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
        {
            var notification = await _unitOfWork.NotificationRepository.GetFirstOrDefaultAsync(n => n.Id == request.NotificationID && n.RecipientId == request.UserID);

            if(notification == null)
            {
                return GeneralResponse<string>.FailResponse("Notification not found");
            }

            if (!notification.IsRead)
                return GeneralResponse<string>.FailResponse("Notification already marked as read");

            notification.IsRead = true;
            notification.UpdatedAt= DateTime.Now;

            await _unitOfWork.SaveAsync();
            return GeneralResponse<string>.SuccessResponse("Notification marked as read");
        }
    }
}

