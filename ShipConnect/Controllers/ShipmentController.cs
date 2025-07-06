using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShipConnect.CQRS.shipments.Queries;
using ShipConnect.CQRS.Shipments.Commands;
using ShipConnect.CQRS.Shipments.Queries;
using ShipConnect.DTOs.ShipmentDTOs;
using ShipConnect.Helpers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

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

        #region ShippingCompany

        [Authorize(Roles = "ShippingCompany")]
        [HttpGet("ShippingCompany/Id/{Id:int}")]
        public async Task<IActionResult> GetShippingShipmentById(int Id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var query = new GetShippingShipmentByIdQuery
            {
                UserId = userId,
                ShipmentId = Id,
            };

            var result = await _mediator.Send(query);

            return result.Success ? Ok(result) : BadRequest(result);
        }
        #endregion


        #region StartUp 

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

        [Authorize(Roles = "Startup")]
        [HttpGet("startUp/ShippingMethodCount")]
        public async Task<IActionResult> GetShippingMethodCount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var query = new GetShippingMethodCountQuery
            {
                UserId = userId,
            };

            var result = await _mediator.Send(query);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Startup")]
        [HttpGet("startUp/ShippingScopeCount")]
        public async Task<IActionResult> GetShippingScopeCount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var query = new GetShippingScopeCountQuery
            {
                UserId = userId,
            };

            var result = await _mediator.Send(query);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Startup")]
        [HttpGet("startUp/Id/{Id:int}")]
        public async Task<IActionResult> GetStartUpShipmentById(int Id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var query = new GetStartUpShipmentByIdQuery
            {
                UserId = userId,
                ShipmentId = Id,
            };

            var result = await _mediator.Send(query);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Startup")]
        [HttpPost("startUp/add")]
        public async Task<IActionResult> Addshipment(AddShipmentDTO shipmentDTO)
        {
            if (ModelState.IsValid)
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
        [HttpPost("startUp/edit/{id:int}")]
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
        [HttpDelete("startUp/delete/{id:int}")]
        public async Task<IActionResult> DeleteShipment(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var query = new DeleteShipmentCommand
            {
                UserID = userId,
                ShipmentId = id
            };

            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        #endregion


        #region StartUp & Shipping Company 
        [HttpGet("AllShipments")]
        public async Task<IActionResult> GetAllShipments(int pageNumber = 1, int pageSize = 10)
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

        [HttpGet("filterWithCode/{code}")]
        public async Task<IActionResult> GetShipmentWithCode(string code)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var query = new GetShipmentWithCodeQuery
            {
                UserId = userId,
                Code = code,
            };

            var result = await _mediator.Send(query);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("filterWithStatus/{status:int}")]
        public async Task<IActionResult> GetShipmentsWithStatus(int status, int pageNumber = 1, int pageSize = 10)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var query = new GetShipmentsWithStatusQuery
            {
                UserId = userId,
                Status = status,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("StatusCount")]
        public async Task<IActionResult> StatusCount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var query = new GetAllStatusCountQuery
            {
                UserId = userId,
            };

            var result = await _mediator.Send(query);

            return result.Success ? Ok(result) : BadRequest(result);
        }

       

        #endregion


        #region Admin

        [Authorize(Roles = "Admin")]
        [HttpGet("Admin/StatusCount")]
        public async Task<IActionResult> AdminStatusCount()
        {
            var query = new AdminStatusCountQuery();

            var result = await _mediator.Send(query);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Admin/ShippingCount/{shippingCompanyId:int}")]
        public async Task<IActionResult> AdminShippingStatusCount(int shippingCompanyId)
        {
            var query = new AdminShippingStatusCountQuery
            {
                ShippingCompanyID = shippingCompanyId
            };

            var result = await _mediator.Send(query);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Admin/StatusCount/{startUpId:int}")]
        public async Task<IActionResult> AdminStartUpStatusCount(int startUpId)
        {
            var query = new AdminStartUpStatusCountQuery
            {
                StartUpID = startUpId
            };

            var result = await _mediator.Send(query);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Admin/ShippingMethodCount/{startUpId:int}")]
        public async Task<IActionResult> AdminStartUpShippingMethodCount(int startUpId)
        {
            var query = new AdminStartUpShippingMethodCountQuery
            {
                StartUpID = startUpId
            };

            var result = await _mediator.Send(query);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Admin/ShippingScopeCount/{startUpId:int}")]
        public async Task<IActionResult> AdminStartUpShippingScopeCount(int startUpId)
        {
            var query = new AdminStartUpShippingScopeCountQuery
            {
                StartUpID = startUpId
            };

            var result = await _mediator.Send(query);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        #endregion



    }

}
