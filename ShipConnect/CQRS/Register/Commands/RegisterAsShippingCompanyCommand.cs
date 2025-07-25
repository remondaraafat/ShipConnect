﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using ShipConnect.CQRS.Notification.Commands;
using ShipConnect.DTOs.NotificationDTO;
using ShipConnect.Hubs;
using ShipConnect.Models;

namespace ShipConnect.CQRS.Register.Commands
{
    public class RegisterAsShippingCompanyCommand:IRequest<GeneralResponse<List<string>>>
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        //public string City { get; set; }
        public string Address { get; set; }
        public TransportType TransportType { get; set; }
        public ShippingScope ShippingScope { get; set; }
        public string? LicenseNumber { get; set; }
        public string? Description { get; set; }
        public string? TaxId { get; set; }
        public string? Website { get; set; }
        public bool AcceptTerms { get; set; }

    }

    public class RegisterAsShippingCompanyCommandHandler : IRequestHandler<RegisterAsShippingCompanyCommand, GeneralResponse<List<string>>>
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly IUnitOfWork UnitOfWork;
        private readonly IEmailService _emailService;
        private readonly IMediator mediator;
        private readonly IHubContext<NotificationHub> hubContext;

        public RegisterAsShippingCompanyCommandHandler(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IEmailService emailService,IMediator mediator, IHubContext<NotificationHub> hubContext)
        {
            this.UserManager = userManager;
            this.UnitOfWork = unitOfWork;
            this._emailService = emailService;
            this.mediator = mediator;
            this.hubContext = hubContext;
        }

        public async Task<GeneralResponse<List<string>>> Handle(RegisterAsShippingCompanyCommand request, CancellationToken cancellationToken)
        {
            if(!request.AcceptTerms)
                return GeneralResponse<List<string>>.FailResponse("You must accept the terms and conditions.");

            var existEmail = await UserManager.FindByEmailAsync(request.Email);
            if (existEmail != null)
                return GeneralResponse<List<string>>.FailResponse("Email already exists", new List<string> { "This email is already in use." });

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.Phone,
                Name=request.CompanyName,
                IsApproved = false
            };
            
            //create account
            var result= await UserManager.CreateAsync(user,request.Password);
            if (!result.Succeeded)
            {
                List<string> errorList = result.Errors.Select(e => e.Description).ToList();
                return GeneralResponse<List<string>>.FailResponse("Failed to create user", errorList);
            }

            //add role
            var role = await UserManager.AddToRoleAsync(user, UserRole.ShippingCompany.ToString());
            if(!role.Succeeded)
            {
                await UserManager.DeleteAsync(user);//rollback
                List<string> roleErrors = role.Errors.Select(e=>e.Description).ToList();
                return GeneralResponse<List<string>>.FailResponse("Failed to assign role", roleErrors);
            }

            var company = new ShippingCompany
            {
                CompanyName = request.CompanyName,
                Address = request.Address,
                Phone = request.Phone,
                Description = request.Description,
                UserId = user.Id,
                Website = request.Website,
                TaxId = request.TaxId,
                TransportType = request.TransportType,
                ShippingScope = request.ShippingScope,
                LicenseNumber = request.LicenseNumber,
            };

            await UnitOfWork.ShippingCompanyRepository.AddAsync(company);
            await UnitOfWork.SaveAsync();

            var admins = await UserManager.GetUsersInRoleAsync("Admin");

            var notification = new CreateNotificationDTO
            {
                Title = "New Shipping‑Company Account Requires Approval",
                Message = $"{request.CompanyName} just signed up as a shipping company and is waiting for your approval.",
                RecipientIds = admins.Select(u=>u.Id),
                NotificationType = NotificationType.General,
            };

            // 1) الرسالة تُحفظ في جدول Notifications
            await mediator.Send(new CreateNotificationCommand(notification), cancellationToken);

            // 2) تُرسل فورًا عبر SignalR
            await hubContext.Clients.Users(admins.Select(u=>u.Id)).SendAsync("NewApprovalRequest", notification,cancellationToken);

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
