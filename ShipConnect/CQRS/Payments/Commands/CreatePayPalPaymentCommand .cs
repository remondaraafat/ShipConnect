namespace ShipConnect.CQRS.Payments.Commands
{
    public class CreatePayPalPaymentCommand : IRequest<string>
    {
        public decimal Amount { get; set; }
        public int OfferId { get; set; }
        public string Notes { get; set; } = "";
        public string PayerId { get; set; }
    }

    public class CreatePayPalPaymentHandler : IRequestHandler<CreatePayPalPaymentCommand, string>
    {
        private readonly PayPalService _paypalService;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePayPalPaymentHandler(PayPalService paypalService, IUnitOfWork unitOfWork)
        {
            _paypalService = paypalService;
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Handle(CreatePayPalPaymentCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var currency = "USD"; // العملة ثابتة

                var (orderId, paymentUrl) = await _paypalService.CreateOrder(command.Amount, currency);

                var payment = new Payment
                {
                    Amount = command.Amount,
                    Currency = currency,
                    OfferId = command.OfferId,
                    PayerId = command.PayerId,
                    PayPalOrderId = orderId,
                    Notes = command.Notes,
                    Status = PaymentStatus.Pending,
                    PaymentMethod = PaymentMethod.PayPal,
                    IsConfirmed = false,
                    PaymentDate = DateTime.UtcNow
                };

                await _unitOfWork.PaymentRepository.AddAsync(payment);
                await _unitOfWork.SaveAsync();

                return paymentUrl;
            }
            catch (Exception ex)
            {
                // مهم جدًا علشان يرجع رسالة واضحة لـ Angular
                throw new Exception($"Error creating PayPal order: {ex.Message}", ex);
            }
        }


    }
}
