using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ShipConnect.Helpers;
using ShipConnect.Models;
using ShipConnect.UnitOfWorkContract;
using static ShipConnect.Enums.Enums;

namespace ShipConnect.CQRS.Register.Commands
{
    public class RegisterAsShippingCompanyCommand:IRequest<GeneralResponse<List<string>>>
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public TransportType TransportType { get; set; }
        public ShippingScope ShippingScope { get; set; }
        public string? LicenseNumber { get; set; }
        public string? Description { get; set; }
        public string? TaxId { get; set; }
        public string? Website { get; set; }
    }

    public class RegisterAsShippingCompanyCommandHandler : IRequestHandler<RegisterAsShippingCompanyCommand, GeneralResponse<List<string>>>
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly IUnitOfWork UnitOfWork;

        public RegisterAsShippingCompanyCommandHandler(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            this.UserManager = userManager;
            this.UnitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<List<string>>> Handle(RegisterAsShippingCompanyCommand request, CancellationToken cancellationToken)
        {
            var existEmail = await UserManager.FindByEmailAsync(request.Email);
            if (existEmail == null)
            {
                return GeneralResponse<List<string>>.FailResponse("Email already exists", new List<string> { "This email is already in use." });
            }

            var user = new ApplicationUser
            {
                UserName = request.CompanyName,
                Email = request.Email,
                PhoneNumber = request.Phone
            };
            
            IdentityResult result= await UserManager.CreateAsync(user,request.Password);
            if (!result.Succeeded)
            {
                List<string> errorList = result.Errors.Select(e => e.Description).ToList();
                return GeneralResponse<List<string>>.FailResponse("Failed to create user", errorList);
            }

            IdentityResult role = await UserManager.AddToRoleAsync(user, UserRole.ShippingCompany.ToString());
            if(!role.Succeeded)
            {
                await UserManager.DeleteAsync(user);
                List<string> roleErrors = role.Errors.Select(e=>e.Description).ToList();
                return GeneralResponse<List<string>>.FailResponse("Failed to assign role", roleErrors);
            }

            var shippingCompany = new ShippingCompany
            {
                CompanyName = request.CompanyName,
                Address = request.Address,
                City = request.City,
                Phone = request.Phone,
                Description = request.Description,
                UserId = user.Id,
                Website = request.Website,
                TaxId = request.TaxId,
                TransportType = request.TransportType,
                ShippingScope = request.ShippingScope,
                LicenseNumber = request.LicenseNumber,
            };

            await UnitOfWork.ShippingCompanyRepository.AddAsync(shippingCompany);
            await UnitOfWork.SaveAsync();

            return GeneralResponse<List<string>>.SuccessResponse("User registered successfully");
        }
    }
}
