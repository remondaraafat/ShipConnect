using ShipConnect.Data;
using ShipConnect.RepositoryContract;

namespace ShipConnect.UnitOfWorkContract
{
    public interface IUnitOfWork : IDisposable
    {
        IBankAccountRepository BankAccountRepository { get; }
        IChatMessageRepository ChatMessageRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        IRatingRepository RatingRepository { get; }
        IOfferRepository OfferRepository { get; }
        IReceiverRepository ReceiverRepository { get; }
        IShipmentRepository ShipmentRepository { get; }
        IShippingCompanyRepository ShippingCompanyRepository { get; }
        IStartUpRepository StartUpRepository { get; }
        ITrackingRepository TrackingRepository { get; }
        IPasswordResetCodeRepository PasswordResetCodeRepository { get; }



        Task SaveAsync();
    }
}
