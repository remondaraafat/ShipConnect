﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShipConnect.CQRS.Ratings.Commands;
using ShipConnect.CQRS.Ratings.Queries;
using ShipConnect.DTOs.RatingDTOs;
using ShipConnect.Helpers;

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
            var response = await _mediator.Send(new CreateRatingCommand(dto));
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetByCompany(int companyId)
        {
            var response = await _mediator.Send(new GetRatingsByCompanyIdQuery(companyId));
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpGet("company/{companyId}/average")]
        public async Task<IActionResult> GetAverageScore(int companyId)
        {
            var response = await _mediator.Send(new GetCompanyRatingAverageQuery(companyId));
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRatingDto dto)
        {
            var response = await _mediator.Send(new UpdateRatingCommand(id, dto));
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteRatingCommand(id));
            return response.Success ? NoContent() : NotFound(response);
        }
    }
}
