using System.Text.RegularExpressions;
using MediatR;
using ShipConnect.CQRS.Notification.Commands;
using ShipConnect.DTOs.NotificationDTO;
using ShipConnect.Helpers;
using ShipConnect.Models;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Shipments.Commands
{
    public class UpdateShipmentStatusCommand : IRequest<GeneralResponse<string>>
    {
        public string UserId { get;}
        public int ShipmentId { get; set; }
        public int ShipmentStatus { get; set; }

        public UpdateShipmentStatusCommand(string userId, int shipmentId,int shipmentStatus) { 
            UserId = userId;
            ShipmentId = shipmentId;
            ShipmentStatus = shipmentStatus;
        }
    }

    public class UpdateShipmentStatusCommandHandler : IRequestHandler<UpdateShipmentStatusCommand, GeneralResponse<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IEmailService _emailService;

        public UpdateShipmentStatusCommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _emailService = emailService;
        }

        public async Task<GeneralResponse<string>> Handle(UpdateShipmentStatusCommand request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.ShippingCompanyRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserId);
            if (company is null)
                return GeneralResponse<string>.FailResponse("User not found");

            if (!Enum.IsDefined(typeof(ShipmentStatus), request.ShipmentStatus))
                return GeneralResponse<string>.FailResponse("Invalid shipment status value");

            var newStatus = (ShipmentStatus)request.ShipmentStatus;

            var query = await _unitOfWork.OfferRepository
                .GetWithFilterAsync(o => o.ShippingCompanyId == company.Id
                    && o.ShipmentId == request.ShipmentId
                    && o.IsAccepted)
                .Include(o => o.Shipment).ThenInclude(o=>o.Startup).ThenInclude(o=>o.User)
                .FirstOrDefaultAsync(cancellationToken);

            if (query is null)
                return GeneralResponse<string>.FailResponse("Shipment not found");
           
            if (newStatus == ShipmentStatus.InTransit)
                query.Shipment.ActualSentDate = DateTime.Now;
            
            query.Shipment.Status = newStatus;

            string formattedStatus = Regex.Replace(newStatus.ToString(), "(\\B[A-Z])", " $1");

            var notification = new CreateNotificationDTO
            {
                RecipientId = query.Shipment.Startup.UserId,
                Title = newStatus == ShipmentStatus.Delivered
                    ? "Shipment Delivered"
                    : "Shipment Status Updated",
                Message = newStatus == ShipmentStatus.Delivered
                    ? $"Your shipment with code {query.Shipment.Code} has been delivered successfully."
                    : $"Your shipment with code {query.Shipment.Code} has been updated to {formattedStatus}.",
                NotificationType = newStatus == ShipmentStatus.Delivered
                    ? NotificationType.ShipmentDelivered
                    : NotificationType.ShipmentStatusChanged
            };

            await _mediator.Send(new CreateNotificationCommand(notification));

            if (newStatus == ShipmentStatus.Delivered)
            {
                query.Shipment.ActualDelivery = DateTime.Now;

                await _emailService.SendEmailAsync(
                    toEmail: query.Shipment.Startup.User.Email,
                    subject: "Shipment Delivered",
                    body: $@"
                        <h2>Dear {query.Shipment.Startup.User.Name},</h2>
                        <p>We are pleased to inform you that your shipment with code <strong>{query.Shipment.Code}</strong> has been successfully delivered.</p>
                        <p>Thank you for using <strong>ShipConnect</strong>. We hope to serve you again soon.</p>
                        <hr />
                        <p><em>This is an automated message. Please do not reply.</em></p>"
                );
            }

            await _unitOfWork.SaveAsync();
            return GeneralResponse<string>.SuccessResponse("Shipment status updated successfully");
        }
    }
}
