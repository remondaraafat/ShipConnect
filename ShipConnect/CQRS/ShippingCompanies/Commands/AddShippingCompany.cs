using MediatR;
using ShipConnect.Models;
using ShipConnect.DTOs.ShippingCompanies;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.ShippingCompanies.Commands
{
    public class CreateShippingCompanyCommand : IRequest<GeneralResponse<ShippingCompanyDto>>
    {
        public CreateShippingCompanyDto Dto { get; set; }

        public CreateShippingCompanyCommand(CreateShippingCompanyDto dto)
        {
            Dto = dto;
        }
    }

    public class CreateShippingCompanyHandler : IRequestHandler<CreateShippingCompanyCommand, GeneralResponse<ShippingCompanyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateShippingCompanyHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<ShippingCompanyDto>> Handle(CreateShippingCompanyCommand request, CancellationToken cancellationToken)
        {
            var entity = new ShippingCompany
            {
                CompanyName = request.Dto.CompanyName,
                Description = request.Dto.Description,
                //City = request.Dto.City,
                Address = request.Dto.Address,
                Phone = request.Dto.Phone,
                Website = request.Dto.Website,
                LicenseNumber = request.Dto.LicenseNumber,
                UserId = request.Dto.UserId,
                TransportType = request.Dto.TransportType,
                ShippingScope = request.Dto.ShippingScope,
                TaxId = request.Dto.TaxId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.ShippingCompanyRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();

            var dto = new ShippingCompanyDto
            {
                Id = entity.Id,
                CompanyName = entity.CompanyName,
                Description = entity.Description,
                //City = entity.City,
                Address = entity.Address,
                Phone = entity.Phone,
                Website = entity.Website,
                LicenseNumber = entity.LicenseNumber,
                UserId = entity.UserId,
                TransportType = entity.TransportType,
                ShippingScope = entity.ShippingScope,
                TaxId = entity.TaxId
            };

            return GeneralResponse<ShippingCompanyDto>.SuccessResponse("Shipping company created successfully", dto);
        }
    }
}
