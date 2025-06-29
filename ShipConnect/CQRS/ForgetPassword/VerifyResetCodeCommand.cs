using MediatR;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.ForgetPassword
{
    public class VerifyResetCodeCommand : IRequest<GeneralResponse<string>>
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }

    public class VerifyResetCodeCommandHandler : IRequestHandler<VerifyResetCodeCommand, GeneralResponse<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public VerifyResetCodeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<string>> Handle(VerifyResetCodeCommand request, CancellationToken cancellationToken)
        {
            var codeEntry = await _unitOfWork.PasswordResetCodeRepository
                .GetFirstOrDefaultAsync(p => p.Email == request.Email && p.Code == request.Code);

            if (codeEntry == null)
                return GeneralResponse<string>.FailResponse("Invalid or expired code");

            if ((DateTime.UtcNow - codeEntry.CreatedAt).TotalMinutes > 10)
                return GeneralResponse<string>.FailResponse("Code expired");

            return GeneralResponse<string>.SuccessResponse("Code verified successfully");
        }
    }

}
