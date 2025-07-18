using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class PayPalService
{
    private readonly PayPalHttpClient _client;
    private readonly ILogger<PayPalService> _logger;

    public PayPalService(IConfiguration config, ILogger<PayPalService> logger)
    {
        _logger = logger;

        var clientId = config["PayPal:ClientId"];
        var secret = config["PayPal:Secret"];

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(secret))
        {
            throw new Exception("PayPal ClientId or Secret not configured properly in appsettings.json");
        }

        var environment = new SandboxEnvironment(clientId, secret);
        _client = new PayPalHttpClient(environment);
    }

    /// <summary>
    /// إنشاء طلب دفع جديد على PayPal
    /// </summary>
    public async Task<(string orderId, string paymentUrl)> CreateOrder(decimal amount, string currency)
    {
        var order = new OrderRequest
        {
            CheckoutPaymentIntent = "CAPTURE",
            PurchaseUnits = new List<PurchaseUnitRequest>
            {
                new PurchaseUnitRequest
                {
                    AmountWithBreakdown = new AmountWithBreakdown
                    {
                        CurrencyCode = currency,
                        Value = amount.ToString("F2")
                    }
                }
            },
            ApplicationContext = new ApplicationContext
            {
                ReturnUrl = "http://localhost:4200/payment",       // مسار الرجوع بعد الدفع
                CancelUrl = "http://localhost:4200/payment?cancel=true" // إلغاء الدفع
            }
        };

        var request = new OrdersCreateRequest();
        request.Prefer("return=representation");
        request.RequestBody(order);

        try
        {
            var response = await _client.Execute(request);
            var result = response.Result<Order>();
            var approveUrl = result.Links.First(x => x.Rel == "approve").Href;

            _logger.LogInformation($"PayPal Order Created. ID: {result.Id}");
            return (result.Id, approveUrl);
        }
        catch (HttpException ex)
        {
            var statusCode = ex.StatusCode;
            var debugId = ex.Headers.GetValues("PayPal-Debug-Id")?.FirstOrDefault();

            _logger.LogError($"PayPal CreateOrder Error - Status: {statusCode}, Debug ID: {debugId}, Message: {ex.Message}");
            throw new Exception("فشل إنشاء عملية الدفع على PayPal. راجع السجلات.");
        }
    }

    /// <summary>
    /// تنفيذ الدفع بعد الموافقة عليه من المستخدم
    /// </summary>
    public async Task<bool> CaptureOrder(string orderId)
    {
        var request = new OrdersCaptureRequest(orderId);
        request.RequestBody(new OrderActionRequest());

        try
        {
            var response = await _client.Execute(request);
            var result = response.Result<Order>();

            _logger.LogInformation($"PayPal Order Captured. ID: {orderId}, Status: {result.Status}");
            return result.Status == "COMPLETED";
        }
        catch (HttpException ex)
        {
            var statusCode = ex.StatusCode;
            var debugId = ex.Headers.GetValues("PayPal-Debug-Id")?.FirstOrDefault();

            _logger.LogError($"PayPal Capture Error - Status: {statusCode}, Debug ID: {debugId}, Message: {ex.Message}");
            return false;
        }
    }
}
