using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShipConnect.CQRS.AdminProfileCQRS.Query;
using ShipConnect.CQRS.UserCQRS.Commands;
using ShipConnect.CQRS.UserCQRS.Query;
using ShipConnect.DTOs.UserDTOs;
using ShipConnect.ShippingCompanies.Querys;

namespace ShipConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminProfileController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdminProfileController(IMediator mediator) => _mediator = mediator;

        #region Admin

        [Authorize(Roles ="Admin")]
        [HttpGet("AllUsers")]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var response = await _mediator.Send(new GetAllUsersQuery(pageNumber, pageSize));
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("me")]
        public async Task<GeneralResponse<GetUserDTO>> GetMyProfile(CancellationToken cancellationToken)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email))
                return GeneralResponse<GetUserDTO>.FailResponse("Email claim is missing in token.");

            var userDto = await _mediator.Send(new GetUserByEmailQuery { Email = email }, cancellationToken);

            if (userDto == null)
                return GeneralResponse<GetUserDTO>.FailResponse("Admin profile not found.");

            return GeneralResponse<GetUserDTO>.SuccessResponse("Profile loaded successfully.", userDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<GeneralResponse<object>> EditMyProfile([FromBody] EditUserDTO dto, CancellationToken cancellationToken)
        {
            string Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(Id))
                return GeneralResponse<object>.FailResponse("Id not found in token.");

            var result = await _mediator.Send(new EditUserCommand(Id,dto), cancellationToken);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return GeneralResponse<object>.FailResponse("Failed to update profile: " + string.Join("; ", errors));
            }

            return GeneralResponse<object>.SuccessResponse("Profile updated successfully.");
        }

        #endregion

    }
}
