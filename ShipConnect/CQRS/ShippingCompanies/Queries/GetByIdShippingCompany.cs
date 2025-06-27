using MediatR;
using ShipConnect.DTOs.ShippingCompanies;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.ShippingCompanies.Querys
{
    public class GetShippingCompanyByIdQuery : IRequest<ShippingCompanyDto>
    {
        public int Id { get; set; }
        public GetShippingCompanyByIdQuery(int id) => Id = id;
    }


    public class GetShippingCompanyByIdHandler : IRequestHandler<GetShippingCompanyByIdQuery, ShippingCompanyDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetShippingCompanyByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ShippingCompanyDto> Handle(GetShippingCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.ShippingCompanyRepository.GetByIdAsync(request.Id);
            if (entity == null) return null!;

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
