using Microsoft.AspNetCore.Mvc;
using ShipConnect.CQRS.Payments.Commands;
using ShipConnect.CQRS.Payments.Queries;

[ApiController]

[Route("api/[controller]")]

    public class PaymentController : Controller
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] CreatePayPalPaymentCommand command)
        {
            var paymentUrl = await _mediator.Send(command);
            return Ok(new { paymentUrl });
        }

        [HttpPost("capture-order/{orderId}")]
        public async Task<IActionResult> CaptureOrder(string orderId)
        {
            var success = await _mediator.Send(new CapturePayPalPaymentCommand { OrderId = orderId });

            if (!success)
                return BadRequest("Payment capture failed");

            return Ok(new { message = "Payment completed successfully" });
        }


        [HttpGet("total-amount")]
        public async Task<IActionResult> GetTotalPaymentsAmount()
        {
            var response = await _mediator.Send(new GetAllPaymentsAmountQuery());
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetPaymentSummary()
        {
            var response = await _mediator.Send(new GetPaymentSummaryQuery());
            return response.Success ? Ok(response) : BadRequest(response);
        }


    }

