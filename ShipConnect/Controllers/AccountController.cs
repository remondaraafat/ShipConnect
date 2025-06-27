using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShipConnect.DTOs.AccountDTOs;
using ShipConnect.Models;
//using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;


namespace ShipConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public UserManager<ApplicationUser> UserManager { get; }
        public IConfiguration Configuration { get; }

        public AccountController(UserManager<ApplicationUser> UserManager,IConfiguration configuration)
        {
            this.UserManager = UserManager;
            Configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO userFromRequest)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = userFromRequest.UserName;   
                user.Email = userFromRequest.Email; 

                IdentityResult result = await UserManager.CreateAsync(user, userFromRequest.Password);
                if (result.Succeeded)
                {
                    return Ok("Created");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("Password", item.Description);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO userFromRequest)
        {
            if(ModelState.IsValid)
            {
                //ckeck
                ApplicationUser userFromDb = await UserManager.FindByNameAsync(userFromRequest.UserName);
                if (userFromDb != null)
                {
                    bool fount = await UserManager.CheckPasswordAsync(userFromDb, userFromRequest.Password);

                    if (fount)
                    {
                        List<Claim> userClaims = new List<Claim>();
                        //Token Generated Id change (JWT
                        userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        userClaims.Add(new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()));
                        userClaims.Add(new Claim(ClaimTypes.Name, userFromDb.UserName));

                        ICollection<string> userRoles = await UserManager.GetRolesAsync(userFromDb);

                        foreach (var roleName in userRoles)
                        {
                            userClaims.Add(new Claim(ClaimTypes.Role, roleName));
                        }

                        var SignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecritKey"]));

                        SigningCredentials signingCredentials = new SigningCredentials(SignInKey, SecurityAlgorithms.HmacSha256);

                        //Design token
                        JwtSecurityToken myToken = new JwtSecurityToken(
                            issuer: Configuration["JWT:IssuerIP"],
                            audience: Configuration["JWT:AudienceIP"],
                            expires: DateTime.UtcNow.AddHours(1),
                            claims: userClaims,
                            signingCredentials: signingCredentials
                        );

                        //generate token
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(myToken),
                            expiration = DateTime.Now.AddHours(1)//myToken.ValidTo
                        });
                    }
                }
                ModelState.AddModelError("UserName", "UserName or Password Invalid");
            }
            return BadRequest(ModelState);

        }
    }
}
