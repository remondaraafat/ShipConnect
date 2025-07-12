using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShipConnect.CQRS.Notification.Commands;
using ShipConnect.CQRS.Notification.Queries;
using ShipConnect.DTOs.NotificationDTO;

namespace ShipConnect.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Notification

        [HttpGet("MyNotifications")]
        public async Task<IActionResult> GetUserNotifications()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            var result = await _mediator.Send(new GetUserNotificationsQuery(userId));

            return result.Success ? Ok(result) : BadRequest(result);
        } 
        #endregion

        //[HttpPut("MarkAsRead/{notificationId:int}")]
        //public async Task<IActionResult> MarkAsRead(int notificationId)
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    var command = new MarkNotificationAsReadCommand(userId, notificationId);
        //    var result = await _mediator.Send(command);

        //    return result.Success ? Ok(result) : BadRequest(result);
        //}

        [HttpPut("GetUnreadNotificationsCount")]
        public async Task<IActionResult> GetUnreadNotificationsCount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _mediator.Send(new GetUnreadNotificationsCountQuery(userId));

            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
