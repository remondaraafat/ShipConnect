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
        //public string City { get; set; }
        public string Address { get; set; }
        public TransportType TransportType { get; set; }
        public ShippingScope ShippingScope { get; set; }
        public string? LicenseNumber { get; set; }
        public string? Description { get; set; }
        public string? TaxId { get; set; }
        public string? Website { get; set; }
        public bool AcceptTerms { get; set; }

    }

    public class RegisterAsShippingCompanyCommandHandler : IRequestHandler<RegisterAsShippingCompanyCommand, GeneralResponse<List<string>>>
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly IUnitOfWork UnitOfWork;
        private readonly IEmailService _emailService;

        public RegisterAsShippingCompanyCommandHandler(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IEmailService emailService)
        {
            this.UserManager = userManager;
            this.UnitOfWork = unitOfWork;
            this._emailService = emailService;

        }

        public async Task<GeneralResponse<List<string>>> Handle(RegisterAsShippingCompanyCommand request, CancellationToken cancellationToken)
        {
            if(!request.AcceptTerms)
            {
                return GeneralResponse<List<string>>.FailResponse("You must accept the terms and conditions to register.");
            }
            var existEmail = await UserManager.FindByEmailAsync(request.Email);
            if (existEmail != null)
            {
                return GeneralResponse<List<string>>.FailResponse("Email already exists", new List<string> { "This email is already in use." });
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.Phone,
                Name=request.CompanyName
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
                //City = request.City,
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

            await _emailService.SendEmailAsync(
            toEmail: request.Email,
            subject: "Welcome to ShipConnect!",
                body: $@"
                <div style='max-width: 600px; margin: auto; font-family: Arial, sans-serif; padding: 30px; background-color: #f9f9f9; border-radius: 10px; border: 1px solid #ddd; color: #333;'>
                    <div style='text-align: center;'>
                        <h1 style='color: #2a7ae2; margin-bottom: 0;'>Welcome to ShipConnect! 🚀</h1>
                        <p style='font-size: 16px; margin-top: 5px;'>Hi <strong>{request.CompanyName}</strong>,</p>
                    </div>
    
                    <div style='margin-top: 30px; font-size: 15px; line-height: 1.7;'>
                        <p>We’re excited to have you on board! Your account has been successfully created.</p>
                        <p>Start now by exploring your dashboard, posting shipments, and receiving offers from verified shipping partners across Egypt and beyond.</p>
        
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='https://shipconnect.com/login' style='background-color: #2a7ae2; color: white; padding: 12px 24px; text-decoration: none; border-radius: 6px; font-weight: bold; display: inline-block;'>Login to Your Dashboard</a>
                        </div>

                        <p>If you have any questions or need help, don't hesitate to reach out to our team at 
                            <a href='mailto:support@shipconnect.com' style='color: #2a7ae2;'>support@shipconnect.com</a>.
                        </p>
                    </div>

                    <hr style='margin: 40px 0; border: none; border-top: 1px solid #eee;' />

                    <footer style='font-size: 13px; color: #888; text-align: center;'>
                        © {DateTime.Now.Year} ShipConnect. All rights reserved.
                    </footer>
                </div>"
        );


            return GeneralResponse<List<string>>.SuccessResponse("User registered successfully");
        }
    }
}
