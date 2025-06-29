using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShipConnect.CQRS.Offers.Commands;
using ShipConnect.CQRS.Offers.Queries;
using ShipConnect.DTOs.OfferDTOs;

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
            var result = await _mediator.Send(new GetOffersByShipmentIdQuery(shipmentId));
            return Ok(result);
        }

        // GET: api/offer/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetOfferByIdQuery(id));
            return result == null ? NotFound() : Ok(result);
        }

        // POST: api/offer
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOfferDto dto)
        {
            var result = await _mediator.Send(new CreateOfferCommand(dto));
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: api/offer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOfferDto dto)
        {
            var result = await _mediator.Send(new UpdateOfferCommand(id, dto));
            return result == null ? NotFound() : Ok(result);
        }

        // DELETE: api/offer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _mediator.Send(new DeleteOfferCommand(id));
            return success ? NoContent() : NotFound();
        }

        [HttpPut("offer-status/{offerId}")]
        public async Task<IActionResult> UpdateOfferStatus(int offerId, [FromQuery] bool isAccepted)
        {
            var result = await _mediator.Send(new UpdateOfferStatusCommand(offerId, isAccepted));
            return result ? Ok() : NotFound();
        }


        [HttpGet("stats/{companyId}")]
        public async Task<IActionResult> GetOfferStatsByCompanyId(int companyId)
        {
            var result = await _mediator.Send(new GetOfferStatsByCompanyIdQuery(companyId));
            return Ok(result);
        }

        [HttpGet("total-count")]
        public async Task<IActionResult> GetTotalOffersCount()
        {
            var count = await _mediator.Send(new GetTotalOffersCountQuery());
            return Ok(new { totalOffers = count });
        }


    }
}
