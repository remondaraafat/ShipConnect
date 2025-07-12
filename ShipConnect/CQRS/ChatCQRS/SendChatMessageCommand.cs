
using Microsoft.AspNetCore.SignalR;
using ShipConnect.DTOs.ChatDTOs;
using ShipConnect.Hubs;

namespace ShipConnect.CQRS.ChatCQRS
{
    public class SendChatMessageCommand : IRequest<SendChatMessageDTO>
    {
        public SendChatMessageRequestDTO DTO { get; set; } 
    }
    public class SendChatMessageCommandHandler : IRequestHandler<SendChatMessageCommand, SendChatMessageDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ChatHub> _hubContext;
        public Task<SendChatMessageDTO> Handle(SendChatMessageCommand request, CancellationToken cancellationToken)
        {
            
        }
    }
}
