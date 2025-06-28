using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

        //private readonly RoleManager<IdentityRole> roleManager;
        //private readonly IConfiguration Configuration;

        public AccountController(IMediator mediator)//,IConfiguration configuration,RoleManager<IdentityRole> roleManager)
        {
            this._mediator = mediator;
            //this.roleManager = roleManager;
            //this.Configuration = configuration;
        }

        [HttpPost("Register/startUp")]
        public async Task<IActionResult> RegisterAsStartUp([FromBody] RegisterAsStartUpDTO userFromRequest)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(GeneralResponse<List<string>>.FailResponse("Validation Failed", errors));
            }

            var command = new RegisterAsStartUpCommand
            {
                StartUpName = userFromRequest.StartUpName,
                Email = userFromRequest.Email,
                Phone = userFromRequest.Phone,
                Password = userFromRequest.Password,
                Address = userFromRequest.Address,
                City = userFromRequest.City,
                BusinessCategory = userFromRequest.BusinessCategory,
                Description = userFromRequest.Description,
                TaxId = userFromRequest.TaxId
            };

            var result = await _mediator.Send(command);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        //[HttpPost("Login")]
        //public async Task<IActionResult> Login(LoginDTO userFromRequest)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        //ckeck
        //        ApplicationUser userFromDb = await UserManager.FindByNameAsync(userFromRequest.UserName);
        //        if (userFromDb != null)
        //        {
        //            bool fount = await UserManager.CheckPasswordAsync(userFromDb, userFromRequest.Password);

        //            if (fount)
        //            {
        //                List<Claim> userClaims = new List<Claim>();
        //                //Token Generated Id change (JWT
        //                userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        //                userClaims.Add(new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()));
        //                userClaims.Add(new Claim(ClaimTypes.Name, userFromDb.UserName));

        //                ICollection<string> userRoles = await UserManager.GetRolesAsync(userFromDb);

        //                foreach (var roleName in userRoles)
        //                {
        //                    userClaims.Add(new Claim(ClaimTypes.Role, roleName));
        //                }

        //                var SignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecritKey"]));

        //                SigningCredentials signingCredentials = new SigningCredentials(SignInKey, SecurityAlgorithms.HmacSha256);

        //                //Design token
        //                JwtSecurityToken myToken = new JwtSecurityToken(
        //                    issuer: Configuration["JWT:IssuerIP"],
        //                    audience: Configuration["JWT:AudienceIP"],
        //                    expires: DateTime.UtcNow.AddHours(1),
        //                    claims: userClaims,
        //                    signingCredentials: signingCredentials
        //                );

        //                //generate token
        //                return Ok(new
        //                {
        //                    token = new JwtSecurityTokenHandler().WriteToken(myToken),
        //                    expiration = DateTime.Now.AddHours(1)//myToken.ValidTo
        //                });
        //            }
        //        }
        //        ModelState.AddModelError("UserName", "UserName or Password Invalid");
        //    }
        //    return BadRequest(ModelState);

        //}

    }
}
