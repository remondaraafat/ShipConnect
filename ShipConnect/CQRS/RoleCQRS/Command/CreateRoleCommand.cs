using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ShipConnect.CQRS.RoleCQRS.Command
{
    public class CreateRoleCommand:IRequest
    {
       public string Role { get; set; }
    }
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand>
    {
        private readonly RoleManager<IdentityRole> roleManager;
        public CreateRoleCommandHandler(RoleManager<IdentityRole> rm)
        { roleManager = rm; }
        
        public Task Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Role))
            {
                return;//BadRequest("Role name is required.");
            }

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
        }

    }
}
