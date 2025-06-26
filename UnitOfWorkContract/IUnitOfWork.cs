using ShipConnect.RepositoryContract;

namespace ShipConnect.UnitOfWorkContract
{
    public interface IUnitOfWork : IDisposable
    {
        IOfferRepository OfferRepository { get; }
        IReceiverRepository ReceiverRepository { get; }
        IShipmentRepository ShipmentRepository { get; }
        IShippingCompanyRepository ShippingCompanyRepository { get; }
        IStartUpRepository StartUpRepository { get; }
        ITrackingRepository TrackingRepository { get; }

        Task SaveAsync();
    }
}
