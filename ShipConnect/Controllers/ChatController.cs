using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShipConnect.CQRS.ChatCQRS.Commands;
using ShipConnect.CQRS.ChatCQRS.Queries;

using ShipConnect.DTOs.ChatDTOs;

namespace ShipConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChatController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("send")]
        public async Task<GeneralResponse<SendChatMessageDTO>> Send([FromBody] SendChatMessageRequestDTO dto)
        {
            var command = new SendChatMessageCommand { DTO = dto };
            return await _mediator.Send(command);
            
        }
        [HttpGet("messages")]
        public async Task<GeneralResponse<PagedResult<GetMessagesByUsersIDs_ShipmentIDDTO>>> GetMessagesByUsersIDs_ShipmentID([FromQuery] GetMessagesByUsersIDs_ShipmentIDRequestDTO dto)
        {
            var query = new GetMessagesByUsersIDs_ShipmentIDQuery { RequeestDTO = dto };
            return await _mediator.Send(query);
        }
    }
}
