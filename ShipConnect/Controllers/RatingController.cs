using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShipConnect.CQRS.Ratings.Commands;
using ShipConnect.CQRS.Ratings.Queries;
using ShipConnect.DTOs.RatingDTOs;

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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRatingDto dto)
        {
            var result = await _mediator.Send(new CreateRatingCommand(dto));
            return Ok(result);
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetByCompany(int companyId)
        {
            var result = await _mediator.Send(new GetRatingsByCompanyIdQuery(companyId));
            return Ok(result);
        }


        [HttpGet("company/{companyId}/average")]
        public async Task<IActionResult> GetAverageScore(int companyId)
        {
            var average = await _mediator.Send(new GetCompanyRatingAverageQuery(companyId));
            return Ok(new { CompanyId = companyId, AverageScore = average });
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRatingDto dto)
        {
            var result = await _mediator.Send(new UpdateRatingCommand(id, dto));
            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _mediator.Send(new DeleteRatingCommand(id));
            return success ? NoContent() : NotFound();
        }
    }

}
