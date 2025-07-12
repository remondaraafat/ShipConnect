using ShipConnect.Models;

namespace ShipConnect.RepositoryContract
{
    public interface INotificationRepository:IGenericRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetUnreadNotification(string userId);
    }
}
