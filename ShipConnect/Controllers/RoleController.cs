using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShipConnect.Models;

namespace ShipConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        public readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("Create")]
        [ProducesDefaultResponseType(typeof(int))]

        public async Task<ActionResult> CreateRoleAsync(string role)
        {
            return Ok(await _mediator.Send());
        }
        //private readonly UserManager<ApplicationUser> userManager;
        //private readonly RoleManager<IdentityRole> roleManager;
        //private readonly IConfiguration config;

        //public RoleController(UserManager<ApplicationUser> userManager, IConfiguration config, RoleManager<IdentityRole> roleManager)
        //{
        //    this.userManager = userManager;
        //    this.config = config;
        //    this.roleManager = roleManager;
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpPost("addrole")]
        //public async Task<IActionResult> CreateRole(string role)
        //{
        //    if (string.IsNullOrWhiteSpace(role))
        //    {
        //        return BadRequest("Role name is required.");
        //    }

        //    var roleExists = await roleManager.RoleExistsAsync(role);
        //    if (roleExists)
        //    {
        //        return BadRequest("Role already exists.");
        //    }

        //    var result = await roleManager.CreateAsync(new IdentityRole(role));
        //    if (result.Succeeded)
        //    {
        //        return Ok("Role created successfully.");
        //    }

        //    return BadRequest(result.Errors);
        //}

       // [Authorize(Roles = "Admin")]
        //[HttpPost("assignrole")]
        //public async Task<IActionResult> AssignRole(string userId, string role)
        //{
        //    if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(role))
        //    {
        //        return BadRequest("UserId and Role are required.");
        //    }

        //    var user = await userManager.FindByIdAsync(userId);
        //    if (user == null)
        //    {
        //        return NotFound("User not found.");
        //    }

        //    var roleExists = await roleManager.RoleExistsAsync(role);
        //    if (!roleExists)
        //    {
        //        return NotFound("Role does not exist.");
        //    }

        //    var result = await userManager.AddToRoleAsync(user, role);
        //    if (result.Succeeded)
        //    {
        //        return Ok("Role assigned to user successfully.");
        //    }

        //    return BadRequest(result.Errors);
        //}
    }
}
