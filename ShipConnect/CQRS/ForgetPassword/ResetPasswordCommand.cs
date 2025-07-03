using MediatR;
using Microsoft.AspNetCore.Identity;
using ShipConnect.Helpers;
using ShipConnect.Models;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.ResetPassword.Commands
{
    public class ResetPasswordCommand : IRequest<GeneralResponse<string>>
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
    }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, GeneralResponse<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return GeneralResponse<string>.FailResponse("User not found");

            var codeEntry = await _unitOfWork.PasswordResetCodeRepository
                .GetFirstOrDefaultAsync(x => x.Email == request.Email && x.Code == request.Code);

            if (codeEntry == null || codeEntry.IsUsed || codeEntry.ExpirationDate < DateTime.Now)
                return GeneralResponse<string>.FailResponse("Invalid or expired code");

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return GeneralResponse<string>.FailResponse("Failed to reset password");
            }

            codeEntry.IsUsed = true;
            await _unitOfWork.SaveAsync();

            return GeneralResponse<string>.SuccessResponse("Password reset successfully");
        }
    }
}
