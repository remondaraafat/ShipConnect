using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipConnect.CQRS.Offers.Commands;
using ShipConnect.CQRS.Offers.Queries;
using ShipConnect.CQRS.Shipments.Queries;
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

        [Authorize(Roles = "ShippingCompany")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOfferDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var response = await _mediator.Send(new CreateOfferCommand(userId, dto));
            return response.Success ? CreatedAtAction(nameof(GetById), new { id = response.Data!.Id }, response)
                : BadRequest(response);

        }

        [Authorize(Roles = "ShippingCompany")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOfferDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var response = await _mediator.Send(new UpdateOfferCommand(userId, id, dto));
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "Startup")]
        [HttpPut("acceptOffer/{Id:int}")]
        public async Task<IActionResult> AcceptOffer(int Id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var response = await _mediator.Send(new AcceptOfferCommand(userId,Id));
            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        [Authorize(Roles = "ShippingCompany")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOffer(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var response = await _mediator.Send(new DeleteOfferCommand(userId, id));
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "ShippingCompany")]
        [HttpGet("total-count")]
        public async Task<IActionResult> GetTotalOffersStatusCount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var response = await _mediator.Send(new GetTotalOffersCountQuery(userId));
            return response.Success ? Ok(response) : BadRequest(response);
        }




        // GET: api/offer/shipment/5
        [HttpGet("shipment/{shipmentId:int}")]
        public async Task<IActionResult> GetByShipmentId(int shipmentId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var response = await _mediator.Send(new GetOffersByShipmentIdQuery(shipmentId));
            return response.Success ? Ok(response) : BadRequest(response);
        }



        // GET: api/offer/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _mediator.Send(new GetOfferByIdQuery(id));
            return response.Success ? Ok(response) : NotFound(response);
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

    }
}
