using ShipConnect.Data;
using ShipConnect.Repository;
using ShipConnect.RepositoryContract;

namespace ShipConnect.UnitOfWorkContract
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private readonly ShipConnectContext _context;

        private IBankAccountRepository _bankAccountRepository;
        private IChatMessageRepository _chatMessageRepository;
        private INotificationRepository _notificationRepository;
        private IPaymentRepository _paymentRepository;
        private IOfferRepository _offerRepository;
        private IReceiverRepository _receiverRepository;
        private IShipmentRepository _shipmentRepository;
        private IShippingCompanyRepository _shippingCompanyRepository;
        private IStartUpRepository _startUpRepository;
        private ITrackingRepository _trackingRepository;
        private IRatingRepository _RatingRepository;


        private IPasswordResetCodeRepository _passwordResetCodeRepository;

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
                if (_receiverRepository == null)
                    _receiverRepository = new ReceiverRepository(_context);
                return _receiverRepository;
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

        public IBankAccountRepository BankAccountRepository
        {
            get
            {
                if (_bankAccountRepository == null)
                    _bankAccountRepository = new BankAccountRepository(_context);
                return _bankAccountRepository;
            }
        }

        public IChatMessageRepository ChatMessageRepository
        {
            get
            {
                if (_chatMessageRepository == null)
                    _chatMessageRepository = new ChatMessageRepository(_context);
                return _chatMessageRepository;
            }
        }

        public INotificationRepository NotificationRepository
        {
            get
            {
                if (_notificationRepository == null)
                    _notificationRepository = new NotificationRepository(_context);
                return _notificationRepository;
            }
        }

        public IPaymentRepository PaymentRepository
        {
            get
            {
                if (_paymentRepository == null)
                    _paymentRepository = new PaymentRepository(_context);
                return _paymentRepository;
            }
        }

        public IRatingRepository RatingRepository
        {
            get
            {
                if (_RatingRepository == null)
                    _RatingRepository = new RatingRepository(_context);
                return _RatingRepository;
            }
        }








        public IPasswordResetCodeRepository PasswordResetCodeRepository
        {
            get
            {
                if (_passwordResetCodeRepository == null)
                    _passwordResetCodeRepository = new PasswordResetCodeRepository(_context);
                return _passwordResetCodeRepository;
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
