using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
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
        private readonly IEmailService emailService;

        public AssignRoleToUserCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IEmailService emailService)
        {
            this.userManager = userManager;
            
            this.roleManager = roleManager;
            this.emailService = emailService;   
        }
        public async Task<GeneralResponse<string>> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
        {

            ApplicationUser user = await userManager.FindByEmailAsync(request.DTO.Email);
            
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
                if (request.DTO.RoleName==UserRole.Admin)
                {
                    try
                    {
                        await emailService.SendEmailAsync(user.Email,
                        "You have been assigned the Admin role",
                        "<p>Congratulations 🎉,<br>You're now an Admin on ShipConnect.</p>");
                    }
                    catch (Exception ex)
                    {
                        
                        //logger.LogError(ex, "Failed sending Admin role email to {Email}", user.Email);
                        
                    }
                }
                return GeneralResponse<string>.SuccessResponse("Role assigned to user successfully.");
            }
            var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
            return GeneralResponse<string>.FailResponse(errors);
           // return GeneralResponse<string>.FailResponse("An Error happen while assigning role to user.");
        }
    }
}
