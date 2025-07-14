
using Microsoft.AspNetCore.SignalR;
using ShipConnect.DTOs.ChatDTOs;
using ShipConnect.Hubs;
using ShipConnect.Models;

namespace ShipConnect.CQRS.ChatCQRS
{
    public class SendChatMessageCommand : IRequest<GeneralResponse<SendChatMessageDTO>>
    {
        public SendChatMessageRequestDTO DTO { get; set; } 
    }
    public class SendChatMessageCommandHandler : IRequestHandler<SendChatMessageCommand, GeneralResponse<SendChatMessageDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ChatHub> _hubContext;
        public async Task<GeneralResponse<SendChatMessageDTO>> Handle(SendChatMessageCommand request, CancellationToken cancellationToken)
        {
            try { 
            ChatMessage msg = new ChatMessage
            {
                Content = request.DTO.Content,
                SenderId = request.DTO.SenderId,
                CreatedAt = DateTime.UtcNow,
                ReceiverId = request.DTO.ReceiverId,
                IsRead = false,
                ShipmentId=request.DTO.ShipmentId
            };
            _unitOfWork.ChatMessageRepository.AddAsync(msg);
            await _unitOfWork.SaveAsync();

            SendChatMessageDTO DTO = new SendChatMessageDTO
            {
                ShipmentId = msg.Id,
                Content = msg.Content,
                SenderId = msg.SenderId,
                ReceiverId = msg.ReceiverId,
                IsRead = msg.IsRead
            };
            await _hubContext.Clients
                .User(msg.ReceiverId)
                .SendAsync("ReceiveMessage", DTO, cancellationToken);

            return GeneralResponse<SendChatMessageDTO>.SuccessResponse(data: DTO);
        }
        catch (Exception ex)
        {
            return GeneralResponse<SendChatMessageDTO>.FailResponse(message: ex.Message);
        }
}
    }
}
