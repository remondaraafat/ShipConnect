using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize("Admin")]
    [HttpGet("Admin/total-amount")]
    public async Task<IActionResult> GetAdminTotalPaymentsAmount()
    {
        var response = await _mediator.Send(new GetAllPaymentsAmountQuery());
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("startupPayments/{startUpId:int}")]
    public async Task<IActionResult> GetTotalPaymentsByStartupId(int startUpId)
    {
        var response = await _mediator.Send(new GeStartUpPaymentsAmountQuery(startUpId, null));
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [Authorize(Roles = "Startup")]
    [HttpGet("startupPayments")]
    public async Task<IActionResult> GetTotalPaymentsByUserId()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
            return Unauthorized();

        var response = await _mediator.Send(new GeStartUpPaymentsAmountQuery(null, userId));
        return response.Success ? Ok(response) : BadRequest(response);
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




    [HttpGet("summary")]
    public async Task<IActionResult> GetPaymentSummary()
    {
        var response = await _mediator.Send(new GetPaymentSummaryQuery());
        return response.Success ? Ok(response) : BadRequest(response);
    }


}

