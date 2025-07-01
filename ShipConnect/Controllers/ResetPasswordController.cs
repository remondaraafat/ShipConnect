using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShipConnect.CQRS.ForgetPassword;
using ShipConnect.CQRS.ResetPassword.Commands;
using ShipConnect.CQRS.SentResetCode;
using ShipConnect.DTOs.ForgetPasswordDTO;
using ShipConnect.DTOs.ResetCodeDTO;
using ShipConnect.Helpers;

namespace ShipConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResetPasswordController : ControllerBase
    {
        public IMediator Mediator { get; }

        public ResetPasswordController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> SendResetCode([FromBody] SendResetCodeDTO resetPassword)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(GeneralResponse<List<string>>.FailResponse("Validation failed", errors));
            }

            var command = new SendResetCodeCommand { Email = resetPassword.Email };
            var result = await Mediator.Send(command);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("verifyCode")]
        public async Task<IActionResult> VerifyResetCode([FromBody] VerifyResetCodeDTO dto)
        {
            var command = new VerifyResetCodeCommand
            {
                Email = dto.Email,
                Code = dto.Code
            };

            var result = await Mediator.Send(command);

            return result.Success ? Ok(result) : BadRequest(result);

        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(GeneralResponse<List<string>>.FailResponse("Validation Failed", errors));
            }

            var command = new ResetPasswordCommand
            {
                Email = model.Email,
                Code = model.Code,
                NewPassword = model.NewPassword
            };

            var result = await Mediator.Send(command);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
