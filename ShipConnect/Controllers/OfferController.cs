using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShipConnect.CQRS.Offers.Commands;
using ShipConnect.CQRS.Offers.Queries;
using ShipConnect.DTOs.OfferDTOs;
using ShipConnect.Helpers;

namespace ShipConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OfferController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OfferController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/offer/shipment/5
        [HttpGet("shipment/{shipmentId}")]
        public async Task<IActionResult> GetByShipmentId(int shipmentId)
        {
            var response = await _mediator.Send(new GetOffersByShipmentIdQuery(shipmentId));
            return response.Success ? Ok(response) : NotFound(response);
        }

        // GET: api/offer/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _mediator.Send(new GetOfferByIdQuery(id));
            return response.Success ? Ok(response) : NotFound(response);
        }

        // POST: api/offer
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOfferDto dto)
        {
            var response = await _mediator.Send(new CreateOfferCommand(dto));
            return response.Success
                ? CreatedAtAction(nameof(GetById), new { id = response.Data!.Id }, response)
                : BadRequest(response);
        }

        // PUT: api/offer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOfferDto dto)
        {
            var response = await _mediator.Send(new UpdateOfferCommand(id, dto));
            return response.Success ? Ok(response) : NotFound(response);
        }

        // DELETE: api/offer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteOfferCommand(id));
            return response.Success ? NoContent() : NotFound(response);
        }

        // PUT: api/offer/offer-status/5?isAccepted=true
        [HttpPut("offer-status/{offerId}")]
        public async Task<IActionResult> UpdateOfferStatus(int offerId, [FromQuery] bool isAccepted)
        {
            var response = await _mediator.Send(new UpdateOfferStatusCommand(offerId, isAccepted));
            return response.Success ? Ok(response) : NotFound(response);
        }

        // GET: api/offer/stats/3
        [HttpGet("stats/{companyId}")]
        public async Task<IActionResult> GetOfferStatsByCompanyId(int companyId)
        {
            var response = await _mediator.Send(new GetOfferStatsByCompanyIdQuery(companyId));
            return response.Success ? Ok(response) : NotFound(response);
        }

        // GET: api/offer/total-count
        [HttpGet("total-count")]
        public async Task<IActionResult> GetTotalOffersCount()
        {
            var response = await _mediator.Send(new GetTotalOffersCountQuery());
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
