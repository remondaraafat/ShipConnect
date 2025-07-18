namespace ShipConnect.CQRS.Payments.Commands
{
    public class CapturePayPalPaymentCommand : IRequest<bool>
    {
        public string OrderId { get; set; }
    }

    public class CapturePayPalPaymentHandler : IRequestHandler<CapturePayPalPaymentCommand, bool>
    {
        private readonly PayPalService _paypalService;
        private readonly IUnitOfWork _unitOfWork;

        public CapturePayPalPaymentHandler(PayPalService paypalService, IUnitOfWork unitOfWork)
        {
            _paypalService = paypalService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CapturePayPalPaymentCommand request, CancellationToken cancellationToken)
        {
            var success = await _paypalService.CaptureOrder(request.OrderId);

            if (!success)
                return false;

            var payment = await _unitOfWork.PaymentRepository
                .GetFirstOrDefaultAsync(p => p.PayPalOrderId == request.OrderId);

            if (payment != null)
            {
                payment.Status = PaymentStatus.Completed;
                payment.IsConfirmed = true;
                await _unitOfWork.SaveAsync();
            }

            return true;
        }
    }
}
