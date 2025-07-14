using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShipConnect.CQRS.Offers.Commands;
using ShipConnect.CQRS.ShippingCompanies.Queries;
using ShipConnect.CQRS.StartUps;
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

        #region Admin
        [Authorize(Roles = "Admin")]
        [HttpGet("total-count")]
        public async Task<IActionResult> GetTotalStartUpCount()
        {
            var response = await _mediator.Send(new GetTotalStartUpCountQuery());
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("All")]
        public async Task<IActionResult> GetAllStartups( int pageIndex = 1, int pageSize = 10)
        {
            var response = await _mediator.Send(new GetAllStartupsQuery(pageIndex,pageSize));
            return response.Success ? Ok(response) : BadRequest(response);

        }

        #endregion


        [Authorize]
        [HttpGet] //get by email
        public async Task<IActionResult> GetStartupProfileByEmail([FromQuery] string email, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(
                new GetStartupByEmailQuery { Email = email },
                cancellationToken);

            if (dto == null)
                return NotFound(new { message = "Startup not found." });

            return Ok(dto);
        }
        
        [Authorize]
        [HttpPut] // PUT api/profile/update-my
        public async Task<GeneralResponse<object>> UpdateMyStartupProfile([FromBody] UpdateFullProfileDTO requestDto, CancellationToken cancellationToken)
        {
            string Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        

            if (string.IsNullOrEmpty(Id))
            {
                return GeneralResponse<object>.FailResponse(
                    "Id claim is missing in token."
                );
            }

            var success = await _mediator.Send(
                new UpdateFullProfileOrchestrator
                {
                    Id = Id,
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
        //get all startups
        //[Authorize]

        //get count of startups
        [Authorize]
        [HttpGet("Count")]
        public async Task<GeneralResponse<int>> GetCountOfStartups()
        {
            return GeneralResponse<int>.SuccessResponse("Success", await _mediator.Send(new GetCountOfStartupsQuery())) ;
        }



        //get my startup profile
        [HttpGet("me")]
        public async Task<GeneralResponse<GetStartupByIdDTO>> GetMyStartupProfile(CancellationToken cancellationToken)
        {
            
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(Id))
            {
                return GeneralResponse<GetStartupByIdDTO>.FailResponse(
                    "Email claim is missing in token.");
            }

            
            var dto = await _mediator.Send(
                new GetStartupByIDQuery { Id = Id },
                cancellationToken);

            if (dto == null)
            {
                return GeneralResponse<GetStartupByIdDTO>.FailResponse("Profile not found.");
            }

            return GeneralResponse<GetStartupByIdDTO>.SuccessResponse("Profile loaded", dto);
        }

    }
}
