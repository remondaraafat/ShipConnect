using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShipConnect.CQRS.Login.Commands;
using ShipConnect.CQRS.Register.Commands;
using ShipConnect.DTOs.AccountDTOs;
using ShipConnect.Helpers;
using ShipConnect.Models;
//using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;


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
                    City = userFromRequest.City,
                    Website = userFromRequest.Website,
                    BusinessCategory = userFromRequest.BusinessCategory,
                    Description = userFromRequest.Description,
                    TaxId = userFromRequest.TaxId
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
                    City = userFromRequest.City,
                    TransportType = userFromRequest.TransportType,
                    ShippingScope = userFromRequest.ShippingScope,
                    LicenseNumber = userFromRequest.LicenseNumber,
                    Description = userFromRequest.Description,
                    TaxId = userFromRequest.TaxId,
                    Website = userFromRequest.Website,
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

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO userFromRequest)
        {
            if (ModelState.IsValid)
            {
                var command = new LoginCommand
                {
                    Email = userFromRequest.Email,
                    Password = userFromRequest.Password
                };

                var result = await _mediator.Send(command);

                return result.Success? Ok(result) : Unauthorized(result);

            }
            var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

            return BadRequest(GeneralResponse<List<string>>.FailResponse("Validation Failed", errors));
        }
    }
}
