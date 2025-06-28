using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ShipConnect.Helpers;
using ShipConnect.Models;
using ShipConnect.UnitOfWorkContract;
using static ShipConnect.Enums.Enums;

namespace ShipConnect.CQRS.Register.Commands
{
    public class RegisterAsStartUpCommand:IRequest<GeneralResponse<List<string>>>
    {
        public string StartUpName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string BusinessCategory { get; set; }
        public string? Description { get; set; }
        public string? TaxId { get; set; }

    }

    public class RegisterAsStartUpCommandHandler : IRequestHandler<RegisterAsStartUpCommand, GeneralResponse<List<string>>>
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly IUnitOfWork UnitOfWork;

        public RegisterAsStartUpCommandHandler(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            this.UserManager = userManager;
            this.UnitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<List<string>>> Handle(RegisterAsStartUpCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                UserName = request.StartUpName,
                Email = request.Email,
                PhoneNumber = request.Phone,
                //Role = UserRole.Startup
            };

            IdentityResult result = await UserManager.CreateAsync(user, request.Password);
            
            if (!result.Succeeded)
            {
                List<string> errorList = result.Errors.Select(e=>e.Description).ToList();
                return GeneralResponse<List<string>>.FailResponse("Failed to create user", errorList);
            }
                

            var startUp = new StartUp
            {
                CompanyName = request.StartUpName,
                Address = request.Address,
                City = request.City,
                BusinessCategory = request.BusinessCategory,
                Phone = request.Phone,
                Description = request.Description,
                UserId = user.Id,
                TaxId = request.TaxId,
            };
    
            await UnitOfWork.StartUpRepository.AddAsync(startUp);
            await UnitOfWork.SaveAsync();

            return GeneralResponse<List<string>>.SuccessResponse("User registered successfully");
        }
    }

}
