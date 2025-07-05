using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShipConnect.CQRS.AdminProfileCQRS.Query;
using ShipConnect.CQRS.UserCQRS.Commands;
using ShipConnect.DTOs.UserDTOs;

namespace ShipConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminProfileController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdminProfileController(IMediator mediator) => _mediator = mediator;
        //get
        [HttpGet("me")]
        public async Task<GeneralResponse<GetUserDTO>> GetMyProfile(CancellationToken cancellationToken)
        {
            // ✅ استخرج البريد الإلكتروني من التوكن
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email))
            {
                return GeneralResponse<GetUserDTO>.FailResponse("Email claim is missing in token.");
            }

            // 🔍 نفّذ الكويري باستخدام MediatR
            var userDto = await _mediator.Send(new GetUserByEmailQuery { Email = email }, cancellationToken);

            if (userDto == null)
            {
                return GeneralResponse<GetUserDTO>.FailResponse("Admin profile not found.");
            }

            return GeneralResponse<GetUserDTO>.SuccessResponse("Profile loaded successfully.", userDto);
        }
        //edit
        [HttpPut]
        public async Task<GeneralResponse<object>> EditMyProfile(
        [FromBody] EditUserDTO dto,
        CancellationToken cancellationToken)
        {
            // جلب البريد من التوكن
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                return GeneralResponse<object>.FailResponse("Email not found in token.");
            }

            // استدعاء الأمر
            var result = await _mediator.Send(new EditUserCommand
            {
                Email = email,
                DTO = dto
            }, cancellationToken);

            if (!result.Succeeded)
            {
                // تجهيز قائمة الأخطاء إن وُجدت
                var errors = result.Errors.Select(e => e.Description);
                return GeneralResponse<object>.FailResponse("Failed to update profile: " + string.Join("; ", errors));
            }

            return GeneralResponse<object>.SuccessResponse("Profile updated successfully.");
        }
    }
}
