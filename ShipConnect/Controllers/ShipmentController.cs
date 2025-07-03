using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShipConnect.CQRS.Shipments.Commands;
using ShipConnect.CQRS.Shipments.Queries;
using ShipConnect.DTOs.ShipmentDTOs;
using ShipConnect.Helpers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ShipConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ShipmentController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllShipments(int pageNumber = 1,  int pageSize = 10)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var query = new GetAllShipmentsQuery
            {
                UserId = userId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Startup")]
        [HttpPost("addShipment")]
        public async Task<IActionResult> Addshipment(AddShipmentDTO shipmentDTO)
        {
            if(ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var command = new AddShipmentCommand
                {
                    //Title = shipmentDTO.Title,
                    WeightKg = shipmentDTO.WeightKg,
                    Dimensions = shipmentDTO.Dimensions,
                    Quantity = shipmentDTO.Quantity,
                    Price = shipmentDTO.Price,
                    //DestinationCity = shipmentDTO.DestinationCity,
                    DestinationAddress = shipmentDTO.DestinationAddress,
                    TransportType = shipmentDTO.TransportType,
                    ShipmentType = shipmentDTO.ShipmentType,
                    ShippingScope = shipmentDTO.ShippingScope,
                    Description = shipmentDTO.Description,
                    Packaging = shipmentDTO.Packaging,
                    PackagingOptions = shipmentDTO.PackagingOptions,
                    RequestedPickupDate = shipmentDTO.RequestedPickupDate,
                    SenderPhone = shipmentDTO.SenderPhone,
                    //SenderCity = shipmentDTO.SenderCity,
                    SenderAddress = shipmentDTO.SenderAddress,
                    SentDate = shipmentDTO.SentDate,
                    RecipientName = shipmentDTO.RecipientName,
                    RecipientPhone = shipmentDTO.RecipientPhone,
                    RecipientEmail = shipmentDTO.RecipientEmail,
                    //ReceiverNotes = shipmentDTO.ReceiverNotes,
                    UserId = userId
                };

                var result = await _mediator.Send(command);

                return result.Success ? Ok(result) : BadRequest(result);

            }
            var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

            return BadRequest(GeneralResponse<List<string>>.FailResponse("Validation Failed", errors));

        }

        [Authorize(Roles = "Startup")]
        [HttpPost("editShipment/{id:int}")]
        public async Task<IActionResult> Editshipment(int id, EditShipmentDTO shipmentDTO)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var command = new EditShipmentCommand
                {
                    ShipmentID = id,
                    //Title = shipmentDTO.Title,
                    WeightKg = shipmentDTO.WeightKg,
                    Dimensions = shipmentDTO.Dimensions,
                    Quantity = shipmentDTO.Quantity,
                    Price = shipmentDTO.Price,
                    //DestinationCity = shipmentDTO.DestinationCity,
                    DestinationAddress = shipmentDTO.DestinationAddress,
                    TransportType = shipmentDTO.TransportType,
                    ShipmentType = shipmentDTO.ShipmentType,
                    ShippingScope = shipmentDTO.ShippingScope,
                    Description = shipmentDTO.Description,
                    PackagingOptions = shipmentDTO.PackagingOptions,
                    Packaging = shipmentDTO.Packaging,
                    RequestedPickupDate = shipmentDTO.RequestedPickupDate,
                    SenderPhone = shipmentDTO.SenderPhone,
                    //SenderCity = shipmentDTO.SenderCity,
                    SenderAddress = shipmentDTO.SenderAddress,
                    SentDate = shipmentDTO.SentDate,
                    RecipientName = shipmentDTO.RecipientName,
                    RecipientPhone = shipmentDTO.RecipientPhone,
                    RecipientEmail = shipmentDTO.RecipientEmail,
                    //ReceiverNotes = shipmentDTO.ReceiverNotes,
                    UserId = userId
                };

                var result = await _mediator.Send(command);

                return result.Success ? Ok(result) : BadRequest(result);

            }
            var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

            return BadRequest(GeneralResponse<List<string>>.FailResponse("Validation Failed", errors));

        }

        [Authorize(Roles = "Startup")]
        [HttpGet("Count/{Status:int}")]
        public async Task<IActionResult> ShipmentStatusCountStartUp(int Status = -1)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var query = new StartUpShipmentStatusCountQuery
            {
                UserId = userId,
                ShipmentStatus = Status
            };

            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("CountAll/{Status:int}")]
        public async Task<IActionResult> AllShipmentsCount(int Status = -1)
        {
            var query = new AllShipmentsCountQuery
            {
                ShipmentStatus = Status
            };

            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        //[Authorize(Roles = "Startup")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteShipment(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var query = new DeleteShipmentCommand
            {
                UserID = userId,
                ShipmentId = id
            };

            var result = await _mediator.Send(query);
            return result.Success ? Ok(result):BadRequest(result);
        }


    }

}
