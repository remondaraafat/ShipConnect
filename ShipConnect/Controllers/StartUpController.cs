using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShipConnect.CQRS.StartUps.Orchestrator;
using ShipConnect.CQRS.StartUps.Queries;

namespace ShipConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StartUpController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StartUpController(IMediator mediator) => _mediator = mediator;
        //get by email
        [Authorize]
        [HttpGet("by-email")]
        public async Task<IActionResult> GetStartupProfileByEmail(
        [FromQuery] string email,
        CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(
                new GetStartupByEmailQuery { Email = email },
                cancellationToken);

            if (dto == null)
                return NotFound(new { message = "Startup not found." });

            return Ok(dto);
        }
        //get my startup profile
        [HttpGet("me")]
        public async Task<GeneralResponse<GetStartupByEmailDTO>> GetMyStartupProfile(CancellationToken cancellationToken)
        {
            
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                return GeneralResponse<GetStartupByEmailDTO>.FailResponse(
                    "Email claim is missing in token.");
            }

            
            var dto = await _mediator.Send(
                new GetStartupByEmailQuery { Email = email },
                cancellationToken);

            if (dto == null)
            {
                return GeneralResponse<GetStartupByEmailDTO>.FailResponse("Profile not found.");
            }

            return GeneralResponse<GetStartupByEmailDTO>.SuccessResponse("Profile loaded", dto);
        }
        // PUT api/profile/update-my
        [Authorize]
        [HttpPut("update-my")]
        public async Task<GeneralResponse<object>> UpdateMyStartupProfile(
            [FromBody] UpdateFullProfileDTO requestDto,
            CancellationToken cancellationToken)
        {
            var email = User.FindFirstValue(ClaimTypes.Email)
                        ?? User.FindFirst("email")?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return GeneralResponse<object>.FailResponse(
                    "Email claim is missing in token."
                );
            }

            var success = await _mediator.Send(
                new UpdateFullProfileOrchestrator
                {
                    Email = email,
                    DTO = requestDto
                },
                cancellationToken
            );

            if (!success)
            {
                return GeneralResponse<object>.FailResponse(
                    "Failed to update profile. One or more operations did not succeed."
                );
            }

            return GeneralResponse<object>.SuccessResponse(
                "Profile updated successfully."
            );
        }

    }
}
