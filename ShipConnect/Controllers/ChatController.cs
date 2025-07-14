using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShipConnect.CQRS.ChatCQRS;
using ShipConnect.DTOs.ChatDTOs;

namespace ShipConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
    }
}
