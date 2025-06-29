using ShipConnect.Data;
using ShipConnect.Models;
using ShipConnect.RepositoryContract;

namespace ShipConnect.Repository
{
    public class ChatMessageRepository:GenericRepository<ChatMessage>,IChatMessageRepository
    {
        public ChatMessageRepository(ShipConnectContext context) : base(context)
        {
            
        }
    }
}
