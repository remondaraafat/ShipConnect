using MediatR;
using ShipConnect.DTOs.ShippingCompanies;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.ShippingCompanies.Commands
{
    public class UpdateShippingCompanyCommand : IRequest<GeneralResponse<ShippingCompanyDto>>
    {
        public string UserId { get;}
        public CreateShippingCompanyDto Dto { get; set; }

        public UpdateShippingCompanyCommand(string userId, CreateShippingCompanyDto dto)
        {
            UserId = userId;
            Dto = dto;
        }
    }

    public class UpdateShippingCompanyHandler : IRequestHandler<UpdateShippingCompanyCommand, GeneralResponse<ShippingCompanyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateShippingCompanyHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<ShippingCompanyDto>> Handle(UpdateShippingCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.ShippingCompanyRepository.GetWithFilterAsync(c=>c.UserId==request.UserId&&c.User.IsApproved).Include(c=>c.User).FirstOrDefaultAsync(cancellationToken);

            if (company == null)
                return GeneralResponse<ShippingCompanyDto>.FailResponse("Shipping company not found");

            company.CompanyName = request.Dto.CompanyName;
            company.Description = request.Dto.Description;
            company.Address = request.Dto.Address;
            company.Phone = request.Dto.Phone;
            company.Website = request.Dto.Website;
            company.LicenseNumber = request.Dto.LicenseNumber;
            company.TransportType = request.Dto.TransportType;
            company.ShippingScope = request.Dto.ShippingScope;
            company.TaxId = request.Dto.TaxId;

            if (!string.IsNullOrWhiteSpace(request.Dto.Email))
                company.User.Email = request.Dto.Email;

            _unitOfWork.ShippingCompanyRepository.Update(company);
            await _unitOfWork.SaveAsync();

            var dto = new ShippingCompanyDto
            {
                Id = company.Id,
                CompanyName = company.CompanyName,
                Description = company.Description,
                Address = company.Address,
                Phone = company.Phone,
                Website = company.Website,
                LicenseNumber = company.LicenseNumber,
                TransportType = company.TransportType,
                ShippingScope = company.ShippingScope,
                TaxId = company.TaxId,
                Email = company.User.Email,                
            };

            return GeneralResponse<ShippingCompanyDto>.SuccessResponse("Shipping company updated successfully", dto);
        }
    }
}
