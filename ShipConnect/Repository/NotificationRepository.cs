using ShipConnect.Data;
using ShipConnect.Models;
using ShipConnect.RepositoryContract;

namespace ShipConnect.Repository
{
    public class NotificationRepository:GenericRepository<Notification>,INotificationRepository
    {
        private readonly ShipConnectContext context;

        public NotificationRepository(ShipConnectContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotification(string userId)
        {
            return await context.Notifications.Where(n=>n.RecipientId == userId && !n.IsRead).ToListAsync();
        }
    }
}
