using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using ShipConnect.CQRS.Notification.Commands;
using ShipConnect.DTOs.NotificationDTO;
using ShipConnect.Helpers;
using ShipConnect.Hubs;
using ShipConnect.Models;
using ShipConnect.UnitOfWorkContract;
using static ShipConnect.Enums.Enums;

namespace ShipConnect.CQRS.Register.Commands
{
    public class RegisterAsStartUpCommand:IRequest<GeneralResponse<List<string>>>
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        //public string City { get; set; }
        public string Address { get; set; }
        public string BusinessCategory { get; set; }
        public string? Description { get; set; }
        public string? Website { get; set; }
        public string? TaxId { get; set; }
        public bool AcceptTerms { get; set; } 
    }

    public class RegisterAsStartUpCommandHandler : IRequestHandler<RegisterAsStartUpCommand, GeneralResponse<List<string>>>
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly IUnitOfWork UnitOfWork;
        private readonly IEmailService _emailService;
        private readonly IMediator mediator;
        private readonly IHubContext<NotificationHub> hubContext;

        public RegisterAsStartUpCommandHandler(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IEmailService emailService, IMediator mediator, IHubContext<NotificationHub> hubContext)
        {
            this.UserManager = userManager;
            this.UnitOfWork = unitOfWork;
            this._emailService = emailService;
            this.mediator = mediator;
            this.hubContext = hubContext;
        }

        public async Task<GeneralResponse<List<string>>> Handle(RegisterAsStartUpCommand request, CancellationToken cancellationToken)
        {
            if (!request.AcceptTerms)
                return GeneralResponse<List<string>>.FailResponse("You must accept the terms and conditions.");

            var existEmail = await UserManager.FindByEmailAsync(request.Email);
            if (existEmail != null)
                return GeneralResponse<List<string>>.FailResponse("Email already exists", new List<string> { "This email is already in use." });

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.Phone,
                Name = request.CompanyName,
                IsApproved = false
            };

            //create account
            var result = await UserManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                List<string> errorList = result.Errors.Select(e => e.Description).ToList();
                return GeneralResponse<List<string>>.FailResponse("Failed to create user", errorList);
            }

            //add role
            var role = await UserManager.AddToRoleAsync(user, UserRole.Startup.ToString());
            if (!role.Succeeded)
            {
                await UserManager.DeleteAsync(user);//rollback
                List<string> roleErrors = role.Errors.Select(e => e.Description).ToList();
                return GeneralResponse<List<string>>.FailResponse("Failed to assign role", roleErrors);
            }

            var startUp = new StartUp
            {
                CompanyName = request.CompanyName,
                Address = request.Address,
                BusinessCategory = request.BusinessCategory,
                Phone = request.Phone,
                Description = request.Description,
                UserId = user.Id,
                Website = request.Website,
                TaxId = request.TaxId,
            };

            await UnitOfWork.StartUpRepository.AddAsync(startUp);
            await UnitOfWork.SaveAsync();

            var admins = await UserManager.GetUsersInRoleAsync("Admin");

            var notification = new CreateNotificationDTO
            {
                Title = "New Startup Account Requires Approval",
                Message = $"{request.CompanyName} just signed up as a startup and is waiting for your approval.",
                RecipientIds = admins.Select(u=>u.Id),
                NotificationType = NotificationType.General
            };

            // 1) الرسالة تُحفظ في جدول Notifications
            await mediator.Send(new CreateNotificationCommand(notification), cancellationToken);

            // 2) تُرسل فورًا عبر SignalR
            await hubContext.Clients.Users(admins.Select(a => a.Id)).SendAsync("NewApprovalRequest", notification, cancellationToken);

            // send email to admin
            var emails = admins.Select(u => u.Email);
            foreach (var email in emails)
            {
                await _emailService.SendEmailAsync(
                toEmail: email,
                subject: "New Startup Registration - Approval Needed",
                body: $@"
                        <div style='max-width:600px;margin:auto;font-family:Arial;padding:30px;
                                    background:#f9f9f9;border-radius:10px;border:1px solid #ddd;color:#333'>
                            <h2 style='color:#2a7ae2'>🚛 New Startup Awaiting Approval</h2>
                            <p><strong>Company Name:</strong> {request.CompanyName}</p>
                            <p><strong>Email:</strong> {request.Email}</p>
                            <p><strong>Phone:</strong> {request.Phone}</p>
                            <p><strong>Address:</strong> {request.Address}</p>
                            <p><strong>Business Category:</strong> {request.BusinessCategory}</p>
                            <p><strong>Description:</strong> {(request.Description ?? "N/A")}</p>
                            <p><strong>Website:</strong> {(request.Website ?? "N/A")}</p>
                            <p><strong>Tax ID:</strong> {(request.TaxId ?? "N/A")}</p>
                            <hr style='margin:30px 0;border:none;border-top:1px solid #eee'>
                            <p>
                                You can review and approve this registration from the admin dashboard.
                            </p>
                            <a href='https://yourdomain.com/admin/startups/pending' 
                                style='display:inline-block;margin-top:20px;padding:10px 20px;
                                background-color:#2a7ae2;color:white;border-radius:5px;text-decoration:none'>
                                Review Now
                            </a>
                            <footer style='font-size:13px;color:#888;margin-top:40px;text-align:center'>
                                © {DateTime.Now.Year} ShipConnect. All rights reserved.
                            </footer>
                        </div>"
                );
            }


            //email to user

            await _emailService.SendEmailAsync(
                toEmail: request.Email,
                subject: "Welcome to ShipConnect!",
                body: $@"
                    <div style='max-width:600px;margin:auto;font-family:Arial;padding:30px;
                                background:#f9f9f9;border-radius:10px;border:1px solid #ddd;color:#333'>
                        <h1 style='color:#2a7ae2;text-align:center'>Welcome to ShipConnect! 🚀</h1>
                        <p style='font-size:16px'>Hi <strong>{request.CompanyName}</strong>,</p>
                        <p>Your account has been created and is pending admin approval.</p>
                        <p>We’ll notify you as soon as you’re approved.</p>
                        <hr style='margin:30px 0;border:none;border-top:1px solid #eee'>
                        <footer style='font-size:13px;color:#888;text-align:center'>
                            © {DateTime.Now.Year} ShipConnect. All rights reserved.
                        </footer>
                    </div>"
                );

            return GeneralResponse<List<string>>.SuccessResponse("User registered successfully");
            
        }
    }
}
