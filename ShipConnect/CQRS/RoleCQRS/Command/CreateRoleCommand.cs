using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ShipConnect.DTOs.RoleDTOs;

namespace ShipConnect.CQRS.RoleCQRS.Command
{
    public class CreateRoleCommand : IRequest<GeneralResponse<string>>
    {
        public CreateRoleDTO DTO { get; set; }
    }
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, GeneralResponse<string>>
    {
        private readonly RoleManager<IdentityRole> roleManager;
        public CreateRoleCommandHandler(RoleManager<IdentityRole> rm)
        { roleManager = rm; }

        public async Task<GeneralResponse<string>> Handle(CreateRoleCommand Request, CancellationToken cancellationToken)
        {
          
                if (string.IsNullOrWhiteSpace(Request.DTO.RoleName))
                {
                    return GeneralResponse<string>.FailResponse("Role name shouldn't have white spaces");
                }

                var roleExists = await roleManager.RoleExistsAsync(Request.DTO.RoleName);
                if (roleExists)
                {
                    return GeneralResponse<string>.FailResponse("Role already exists.");
                }

                var result = await roleManager.CreateAsync(new IdentityRole(Request.DTO.RoleName));
                if (result.Succeeded)
                {
                    return GeneralResponse<string>.SuccessResponse("Role created successfully.");
                }

                return GeneralResponse<string>.FailResponse("Role already exists.");

            
        }
    }
}
