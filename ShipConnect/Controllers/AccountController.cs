using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipConnect.CQRS.Login.Commands;
using ShipConnect.CQRS.Register.Commands;
using ShipConnect.CQRS.Register.Queries;
using ShipConnect.DTOs.AccountDTOs;


namespace ShipConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        #region Account

        [HttpPost("register/startUp")]
        public async Task<IActionResult> RegisterAsStartUp([FromBody] RegisterAsStartUpDTO userFromRequest)
        {
            if (ModelState.IsValid)
            {
                var command = new RegisterAsStartUpCommand
                {

                    CompanyName = userFromRequest.CompanyName,
                    Email = userFromRequest.Email,
                    Phone = userFromRequest.Phone,
                    Password = userFromRequest.Password,
                    Address = userFromRequest.Address,
                    //City = userFromRequest.City,
                    Website = userFromRequest.Website,
                    BusinessCategory = userFromRequest.BusinessCategory,
                    Description = userFromRequest.Description,
                    TaxId = userFromRequest.TaxId,
                    AcceptTerms = userFromRequest.AcceptTerms,
                };

                var result = await _mediator.Send(command);

                return result.Success ? Ok(result) : BadRequest(result);
            }
            var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

            return BadRequest(GeneralResponse<List<string>>.FailResponse("Validation Failed", errors));
        }

        [HttpPost("register/shippingCompany")]
        public async Task<IActionResult> RegisterAsShippingCompany([FromBody] RegisterAsShippingCompanyDTO userFromRequest)
        {
            if (ModelState.IsValid)
            {
                var command = new RegisterAsShippingCompanyCommand
                {
                    CompanyName = userFromRequest.CompanyName,
                    Email = userFromRequest.Email,
                    Phone = userFromRequest.Phone,
                    Password = userFromRequest.Password,
                    Address = userFromRequest.Address,
                    //City = userFromRequest.City,
                    TransportType = userFromRequest.TransportType,
                    ShippingScope = userFromRequest.ShippingScope,
                    LicenseNumber = userFromRequest.LicenseNumber,
                    Description = userFromRequest.Description,
                    TaxId = userFromRequest.TaxId,
                    Website = userFromRequest.Website,
                    AcceptTerms = userFromRequest.AcceptTerms
                };

                var result = await _mediator.Send(command);

                return result.Success ? Ok(result) : BadRequest(result);
            }

            var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

            return BadRequest(GeneralResponse<List<string>>.FailResponse("Validation Failed", errors));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("pending-accounts")]
        public async Task<IActionResult> GetPendingAccounts(int PageNumber = 1, int PageSize = 10)
        {
            var response = await _mediator.Send(new GetPendingAccountsQuery(PageNumber, PageSize));
            return response.Success ? Ok(response) : NotFound(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("approve-account/{userId}")]
        public async Task<IActionResult> ApproveAccount(string userId)
        {
            var response = await _mediator.Send(new ApproveAccountCommand(userId));
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO userFromRequest)
        {
            if (ModelState.IsValid)
            {

                var result = await _mediator.Send(new LoginCommand(userFromRequest));

                return result.Success ? Ok(result) : Unauthorized(result);

            }
            var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

            return BadRequest(GeneralResponse<List<string>>.FailResponse("Validation Failed", errors));
        } 

        #endregion
    }
}
