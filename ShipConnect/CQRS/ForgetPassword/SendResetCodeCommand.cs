using MediatR;
using Microsoft.AspNetCore.Identity;
using ShipConnect.Helpers;
using ShipConnect.Models;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.SentResetCode
{
    public class SendResetCodeCommand : IRequest<GeneralResponse<string>>
    {
        public string Email { get; set; }
    }

    public class SendResetCodeCommandHandler : IRequestHandler<SendResetCodeCommand, GeneralResponse<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public SendResetCodeCommandHandler(
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork,
            IEmailService emailService)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<GeneralResponse<string>> Handle(SendResetCodeCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return GeneralResponse<string>.FailResponse("Email not found.");
            }

            // generate random number from 6 digits
            var code = new Random().Next(100000, 999999).ToString();

            var resetCode = new PasswordResetCode
            {
                Email = request.Email,
                Code = code,
                ExpirationDate = DateTime.Now.AddMinutes(10),
                IsUsed = false
            };

            await _unitOfWork.PasswordResetCodeRepository.AddAsync(resetCode);
            await _unitOfWork.SaveAsync();

            await _emailService.SendEmailAsync(
                toEmail: request.Email,
                subject: "Password Reset Verification Code",
                body: $@"
                    <div style='font-family: Arial, sans-serif; padding: 20px; color: #333;'>
                        <h2 style='color: #2a7ae2;'>ShipConnect</h2>
                        <p>Dear user,</p>
                        <p>You requested to reset your password. Use the following verification code:</p>
                        <div style='font-size: 32px; font-weight: bold; color: #2a7ae2; margin: 20px 0;'>{code}</div>
                        <p>This code will expire in <strong>10 minutes</strong>.</p>
                        <p>If you didn't request this, please ignore this email.</p>
                        <br />
                        <p style='font-size: 14px; color: #888;'>ShipConnect Team</p>
                    </div>"
                    );


            return GeneralResponse<string>.SuccessResponse("Verification code sent to your email.");
        }
    }
}
