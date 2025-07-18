using System.ComponentModel.Design;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipConnect.CQRS.Offers.Commands;
using ShipConnect.CQRS.Offers.Queries;
using ShipConnect.CQRS.Shipments.Queries;
using ShipConnect.CQRS.StartUps.Queries;
using ShipConnect.DTOs.OfferDTOs;
using ShipConnect.Helpers;
using ShipConnect.Models;

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

        #region Shipping Company 

        [Authorize(Roles = "ShippingCompany")]
        [HttpGet("status-count")]
        public async Task<IActionResult> GetOfferStatsCount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();
            
            var response = await _mediator.Send(new GetOfferStatusQuery( null, userId));
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "ShippingCompany")]
        [HttpGet("OfferById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();

            var response = await _mediator.Send(new GetOfferByIdQuery(userId,id));
            return response.Success ? Ok(response) : NotFound(response);
        }

        [Authorize(Roles = "ShippingCompany")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOfferDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();

            var response = await _mediator.Send(new CreateOfferCommand(userId, dto));
            return response.Success ? CreatedAtAction(nameof(GetById), new { id = response.Data!.Id }, response)
                : BadRequest(response);

        }

        [Authorize(Roles = "ShippingCompany")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOfferDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();

            var response = await _mediator.Send(new UpdateOfferCommand(userId, id, dto));
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "ShippingCompany")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOffer(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();

            var response = await _mediator.Send(new DeleteOfferCommand(userId, id));
            return response.Success ? Ok(response) : BadRequest(response);
        }

        //// GET: api/offer/shipment/5
        //[HttpGet("shipment/{shipmentId:int}")]
        //public async Task<IActionResult> GetByShipmentId(int shipmentId)
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    var response = await _mediator.Send(new GetOffersByShipmentIdQuery(shipmentId));
        //    return response.Success ? Ok(response) : BadRequest(response);
        //}
        #endregion


        #region StartUp

        [Authorize(Roles = "Startup")]
        [HttpGet("ShipmentOffers")]
        public async Task<IActionResult> GetShipmentWithOffers()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();

            var response = await _mediator.Send(new GetShipmentWithOffersQuery(userId));
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "Startup")]
        [HttpPut("acceptOffer/{Id:int}")]
        public async Task<IActionResult> AcceptOffer(int Id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();

            var response = await _mediator.Send(new AcceptOfferCommand(userId, Id));
            return response.Success ? Ok(response) : BadRequest(response);
        }

        #endregion


        #region Admin

        [Authorize(Roles = "Admin")]
        [HttpGet("Alloffers-count")]
        public async Task<IActionResult> AdminOfferStatsCount()
        {
            var response = await _mediator.Send(new AdminOfferStatsCountQuery());
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("companyOffers/{companyId:int}")]
        public async Task<IActionResult> AdminCompanyOfferStatsCount(int companyId)
        {
            var response = await _mediator.Send(new GetOfferStatusQuery(companyId, null));
            return response.Success ? Ok(response) : NotFound(response);
        }
        #endregion


    }
}
