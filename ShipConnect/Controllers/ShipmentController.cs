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
using ShipConnect.Models;
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

        #region StartUp

        [Authorize(Roles = "Startup")]
        [HttpGet("startUp/Id/{Id:int}")]
        public async Task<IActionResult> GetStartUpShipmentById(int Id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized(); 

            var result = await _mediator.Send(new GetStartUpShipmentByIdQuery(userId, Id));
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Startup")]
        [HttpGet("startUp/ShippingMethodCount")]
        public async Task<IActionResult> GetShippingMethodCount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();

            var result = await _mediator.Send(new GetShippingMethodCountQuery(userId));
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Startup")]
        [HttpGet("startUp/ShippingScopeCount")]
        public async Task<IActionResult> GetShippingScopeCount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized(); 
           
            var result = await _mediator.Send(new GetShippingScopeCountQuery(userId));
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Startup")]
        [HttpGet("Performance")]
        public async Task<IActionResult> GetMonthlyDeliveryPerformance()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();

            var result = await _mediator.Send(new GetMonthlyDeliveryPerformanceQuery(userId));
            return Ok(result);
        }

        [Authorize(Roles = "Startup")]
        [HttpPost("startUp/add")]
        public async Task<IActionResult> Addshipment(AddShipmentDTO shipmentDTO)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                    return Unauthorized();

                var result = await _mediator.Send(new AddShipmentCommand(userId,shipmentDTO));
                return result.Success ? Ok(result) : BadRequest(result);

            }
            var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

            return BadRequest(GeneralResponse<List<string>>.FailResponse("Validation Failed", errors));
        }

        [Authorize(Roles = "Startup")]
        [HttpPut("startUp/edit/{id:int}")]
        public async Task<IActionResult> Editshipment(int id, EditShipmentDTO shipmentDTO)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                    return Unauthorized();

                var result = await _mediator.Send(new EditShipmentCommand(userId, id, shipmentDTO));
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
            if (userId is null)
                return Unauthorized();

            var result = await _mediator.Send(new DeleteShipmentCommand(userId, id));
            return result.Success ? Ok(result) : BadRequest(result);
        }
        #endregion

        
        #region ShippingCompany

        [Authorize(Roles = "ShippingCompany")]
        [HttpGet("ShippingCompany/{Id:int}")]
        public async Task<IActionResult> GetShippingShipmentById(int Id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();

            var result = await _mediator.Send(new GetShippingShipmentByIdQuery(userId, Id));
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "ShippingCompany")]
        [HttpGet("ShipmentRequests")]
        public async Task<IActionResult> GetShipmentRequests(int pageNumber = 1, int pageSize = 10)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();

            var result = await _mediator.Send(new GetShipmentRequestsQuery(userId, pageNumber, pageSize));
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "ShippingCompany")]
        [HttpGet("ShipmentDetails/{shipmentId:int}")]
        public async Task<IActionResult> GetShipmentDetails(int shipmentId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();

            var result = await _mediator.Send(new GetShipmentDetailsQuery(userId, shipmentId));
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "ShippingCompany")]
        [HttpGet("UpdateStatus/{ShipmentId:int}")]
        public async Task<IActionResult> UpdateShipmentStatus(int ShipmentId, int Status)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();

            var result = await _mediator.Send(new UpdateShipmentStatusCommand(userId, ShipmentId, Status));
            return result.Success ? Ok(result) : BadRequest(result);
        }
        
        #endregion
        
        
        #region StartUp & Shipping Company 

        [HttpGet("AllShipments")]
        public async Task<IActionResult> GetAllShipments(int pageNumber = 1, int pageSize = 10)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();

            var result = await _mediator.Send(new GetAllShipmentsQuery(userId, pageNumber, pageSize));
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("filterWithCode/{code}")]
        public async Task<IActionResult> GetShipmentWithCode(string code)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized(); 

            var result = await _mediator.Send(new GetShipmentWithCodeQuery(userId, code));
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("filterWithStatus/{status:int}")]
        public async Task<IActionResult> GetShipmentsWithStatus(int status, int pageNumber = 1, int pageSize = 10)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized(); 
        
            var result = await _mediator.Send(new GetShipmentsWithStatusQuery(userId, status,pageNumber,pageNumber));
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("StatusCount")]
        public async Task<IActionResult> StatusCount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();

            var result = await _mediator.Send(new GetAllStatusCountQuery(userId));
            return result.Success ? Ok(result) : BadRequest(result);
        }

        #endregion


        #region Admin

        [Authorize(Roles = "Admin")]
        [HttpGet("Admin/StatusCount")]
        public async Task<IActionResult> AdminStatusCount()
        {
            var result = await _mediator.Send(new AdminStatusCountQuery());
            return result.Success ? Ok(result) : BadRequest(result);
        }


        #endregion

















        #region Admin


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
