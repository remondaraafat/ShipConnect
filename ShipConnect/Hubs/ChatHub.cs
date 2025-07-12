using Microsoft.AspNetCore.SignalR;
using ShipConnect.CQRS.ChatCQRS;
using ShipConnect.DTOs.ChatDTOs;

namespace ShipConnect.Hubs
{
    public class ChatHub:Hub
    {
        private readonly IMediator _mediator;
        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }
        public Task SendMessage(SendChatMessageRequestDTO DTO)
        {
            return _mediator.Send(new SendChatMessageCommand{DTO = DTO});
        }
    }
}
