using MediatR;
using ShipConnect.DTOs.ShippingCompanies;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.ShippingCompanies.Commands
{
    public class UpdateShippingCompanyCommand : IRequest<ShippingCompanyDto>
    {
        public int Id { get; set; }
        public CreateShippingCompanyDto Dto { get; set; }
        public UpdateShippingCompanyCommand(int id, CreateShippingCompanyDto dto)
        {
            Id = id;
            Dto = dto;
        }
    }

    public class UpdateShippingCompanyHandler : IRequestHandler<UpdateShippingCompanyCommand, ShippingCompanyDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateShippingCompanyHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ShippingCompanyDto> Handle(UpdateShippingCompanyCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.ShippingCompanyRepository.GetByIdAsync(request.Id);
            if (entity == null) return null!;

            entity.CompanyName = request.Dto.CompanyName;
            entity.Description = request.Dto.Description;
            entity.City = request.Dto.City;
            entity.Address = request.Dto.Address;
            entity.Phone = request.Dto.Phone;
            entity.Website = request.Dto.Website;
            entity.LicenseNumber = request.Dto.LicenseNumber;
            entity.UserId = request.Dto.UserId;
            entity.TransportType = request.Dto.TransportType;
            entity.ShippingScope = request.Dto.ShippingScope;
            entity.TaxId = request.Dto.TaxId;

            _unitOfWork.ShippingCompanyRepository.Update(entity);
            await _unitOfWork.SaveAsync();

            return new ShippingCompanyDto
            {
                Id = entity.Id,
                CompanyName = entity.CompanyName,
                Description = entity.Description,
                City = entity.City,
                Address = entity.Address,
                Phone = entity.Phone,
                Website = entity.Website,
                LicenseNumber = entity.LicenseNumber,
                UserId = entity.UserId,
                TransportType = entity.TransportType,
                ShippingScope = entity.ShippingScope,
                TaxId = entity.TaxId
            };
        }
    }



}
