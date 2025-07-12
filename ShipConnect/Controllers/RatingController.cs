using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipConnect.CQRS.Ratings.Commands;
using ShipConnect.CQRS.Ratings.Queries;
using ShipConnect.DTOs.RatingDTOs;
using ShipConnect.Helpers;
using ShipConnect.Models;

namespace ShipConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RatingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region startUp

        [Authorize(Roles = "Startup")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRatingDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();

            var response = await _mediator.Send(new CreateRatingCommand(userId, dto));
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "ShippingCompany")]
        [HttpGet("LastRate")]
        public async Task<IActionResult> LastRate()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();

            var response = await _mediator.Send(new LastRateQuery(userId));
            return response.Success ? Ok(response) : BadRequest(response);
        }

        #endregion

        [HttpGet("ShipmentRate/{ShipmentId:int}")]
        public async Task<IActionResult> GetShipmentRate(int ShipmentId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();

            var response = await _mediator.Send(new GetShipmentRatingQuery(userId, ShipmentId));
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("company/{companyId}/average")]
        public async Task<IActionResult> GetAverageScore(int companyId)
        {
            var response = await _mediator.Send(new GetCompanyRatingAverageQuery(companyId));
            return response.Success ? Ok(response) : NotFound(response);
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var response = await _mediator.Send(new DeleteRatingCommand(id));
        //    return response.Success ? NoContent() : NotFound(response);
        //}
    }
}
