using MediatR;
using Microsoft.AspNetCore.Identity;
using ShipConnect.Models;


namespace ShipConnect.CQRS.RoleCQRS.Command
{
    public class AssignRoleToUserCommand : IRequest<GeneralResponse<string>>
    {
        public AssignRoleToUserDTO DTO { get; set; }
    }
    public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, GeneralResponse<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
       

        public AssignRoleToUserCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            
            this.roleManager = roleManager;
        }
        public async Task<GeneralResponse<string>> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.DTO.UserId) )
            {
                return await Task.FromResult(GeneralResponse<string>.FailResponse("UserId and Role name shouldn't have white spaces."));
            }

            var user = await userManager.FindByIdAsync(request.DTO.UserId);
            if (user == null)
            {
                return GeneralResponse<string>.FailResponse("User not found.");
            }

            var roleExists = await roleManager.RoleExistsAsync(request.DTO.RoleName.ToString());
            if (!roleExists)
            {
                return GeneralResponse<string>.FailResponse("Role does not exist.");
            }

            var result = await userManager.AddToRoleAsync(user, request.DTO.RoleName.ToString());
            if (result.Succeeded)
            {
                return GeneralResponse<string>.SuccessResponse("Role assigned to user successfully.");
            }
            var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
            return GeneralResponse<string>.FailResponse(errors);
           // return GeneralResponse<string>.FailResponse("An Error happen while assigning role to user.");
        }
    }
}
