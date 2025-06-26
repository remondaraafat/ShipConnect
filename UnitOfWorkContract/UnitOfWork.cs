using ShipConnect.Data;
using ShipConnect.Repository;
using ShipConnect.RepositoryContract;

namespace ShipConnect.UnitOfWorkContract
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private readonly ShipConnectContext _context;

        private IOfferRepository _offerRepository;
        private IReceiverRepository _receivderRepository;
        private IShipmentRepository _shipmentRepository;
        private IShippingCompanyRepository _shippingCompanyRepository;
        private IStartUpRepository _startUpRepository;
        private ITrackingRepository _trackingRepository;

        public UnitOfWork(ShipConnectContext context)
        {
            _context = context;
        }

        public IOfferRepository OfferRepository
        {
            get
            {
                if(_offerRepository == null)
                    _offerRepository = new OfferRepository(_context);
                return _offerRepository;
            }
        }

        public IReceiverRepository ReceiverRepository{
            get
            {
                if (_receivderRepository == null)
                    _receivderRepository = new ReceiverRepository(_context);
                return _receivderRepository;
            }
        }

        public IShipmentRepository ShipmentRepository
        {
            get
            {
                if (_shipmentRepository == null)
                    _shipmentRepository = new ShipmentRepository(_context);
                return _shipmentRepository;
            }
        }

        public IShippingCompanyRepository ShippingCompanyRepository
        {
            get
            {
                if (_shippingCompanyRepository == null)
                    _shippingCompanyRepository = new ShippingCompanyRepository(_context);
                return _shippingCompanyRepository;
            }
        }

        public IStartUpRepository StartUpRepository
        {
            get
            {
                if (_startUpRepository == null)
                    _startUpRepository = new StartUpRepository(_context);
                return _startUpRepository;
            }
        }

        public ITrackingRepository TrackingRepository
        {
            get
            {
                if (_trackingRepository == null)
                    _trackingRepository = new TrackingRepository(_context);
                return _trackingRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();  
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
