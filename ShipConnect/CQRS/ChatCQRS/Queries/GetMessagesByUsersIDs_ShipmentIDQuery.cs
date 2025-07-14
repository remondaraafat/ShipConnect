using ShipConnect.DTOs.ChatDTOs;

namespace ShipConnect.CQRS.ChatCQRS.Queries
{
    public class GetMessagesByUsersIDs_ShipmentIDQuery : IRequest<GeneralResponse<PagedResult<GetMessagesByUsersIDs_ShipmentIDDTO>>>
    {
        public GetMessagesByUsersIDs_ShipmentIDRequestDTO RequeestDTO { get; set; } 
        
    }
    public class GetMessagesByUsersIDsShipmentIDQueryHandler : IRequestHandler<GetMessagesByUsersIDs_ShipmentIDQuery, GeneralResponse<PagedResult<GetMessagesByUsersIDs_ShipmentIDDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMessagesByUsersIDsShipmentIDQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GeneralResponse<PagedResult<GetMessagesByUsersIDs_ShipmentIDDTO>>> Handle(GetMessagesByUsersIDs_ShipmentIDQuery request, CancellationToken cancellationToken)
        {
            //var request.RequeestDTO = request.RequeestDTO;
            if (request.RequeestDTO == null)
                return GeneralResponse<PagedResult<GetMessagesByUsersIDs_ShipmentIDDTO>>
                    .FailResponse("Request DTO is null");

            var q = _unitOfWork.ChatMessageRepository.GetWithFilterAsync(m =>
                    m.ShipmentId == request.RequeestDTO.ShipmentId &&
                    (
                        (m.SenderId == request.RequeestDTO.SenderId && m.ReceiverId == request.RequeestDTO.ReceiverId) ||
                        (m.SenderId == request.RequeestDTO.ReceiverId && m.ReceiverId == request.RequeestDTO.SenderId)
                    )
                );

            var totalCount = await q.CountAsync(cancellationToken);

            var items = await q
                .OrderBy(m => m.SentAt)
                .Skip((request.RequeestDTO.PageIndex - 1) * request.RequeestDTO.PageSize)
                .Take(request.RequeestDTO.PageSize)
                .Select(m => new GetMessagesByUsersIDs_ShipmentIDDTO
                {
                    Content = m.Content,
                    SentAt = m.SentAt,
                    IsRead = m.IsRead,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    ShipmentId = m.ShipmentId
                })
                .ToListAsync(cancellationToken);

            var paged = new PagedResult<GetMessagesByUsersIDs_ShipmentIDDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageIndex = request.RequeestDTO.PageIndex,
                PageSize = request.RequeestDTO.PageSize
            };

            return GeneralResponse<PagedResult<GetMessagesByUsersIDs_ShipmentIDDTO>>
                .SuccessResponse(data: paged);
        

        }
    }
}
