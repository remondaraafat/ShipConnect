using Microsoft.AspNetCore.Identity;
using ShipConnect.CQRS.Notification.Commands;
using ShipConnect.DTOs.NotificationDTO;
using ShipConnect.Models;

namespace ShipConnect.CQRS.Login.Commands
{
    public class ApproveAccountCommand:IRequest<GeneralResponse<string>>
    {
        public string UserId { get; }

        public ApproveAccountCommand(string userId)
        {
            UserId = userId;            
        }
    }
    public class ApproveAccountCommandHandler : IRequestHandler<ApproveAccountCommand, GeneralResponse<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;
        private readonly IEmailService emailService;

        public ApproveAccountCommandHandler(UserManager<ApplicationUser> userManager, IMediator mediator, IEmailService emailService)
        {
            _userManager = userManager;
            _mediator = mediator;
            this.emailService = emailService;
        }

        public async Task<GeneralResponse<string>> Handle(ApproveAccountCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null) 
                return GeneralResponse<string>.FailResponse("User not found");

            if(user.IsApproved)
                return GeneralResponse<string>.FailResponse("Account already approved.");

            user.IsApproved = true;
            
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return GeneralResponse<string> .FailResponse("Failed to approve account.");

            await emailService.SendEmailAsync(
                toEmail:user.Email,
                subject: "ShipConnect – Account Approved",
                body: $@"
                <div style='font-family:Arial,Helvetica,sans-serif;font-size:15px;color:#333'>
                    <h2 style='color:#2a7ae2'>Welcome aboard, {user.Name}!</h2>
                    <p>
                        We’re happy to let you know that your ShipConnect account has
                        been <strong>approved</strong>. You now have full access to the platform.
                    </p>
                    <p>
                        <a href='https://shipconnect.com/login'
                           style='background:#2a7ae2;color:#fff;padding:10px 18px;
                                  text-decoration:none;border-radius:5px'>
                            Get started
                        </a>
                    </p>
                    <p style='margin-top:30px;font-size:13px;color:#777'>
                        If you didn’t request this, please ignore this email.
                    </p>
                    <hr/>
                    <p style='font-size:12px;color:#999;text-align:center'>
                        © {DateTime.Now.Year} ShipConnect. All rights reserved.
                    </p>
                </div>");


            var notify = new CreateNotificationDTO
            {
                Title = "Account Approved",
                Message = "Your ShipConnect account has been approved successfully.",
                RecipientId = user.Id,
                NotificationType = NotificationType.General
            };
            await _mediator.Send(new CreateNotificationCommand(notify), cancellationToken);

            return GeneralResponse<string>.SuccessResponse("Account approved and welcome email sent.");


        }
    }

}
