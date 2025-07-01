using MediatR;
using ShipConnect.DTOs.ShippingCompanies;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.ShippingCompanies.Querys
{
    public class GetAllShippingCompaniesQuery : IRequest<GeneralResponse<List<ShippingCompanyDto>>> { }

    public class GetAllShippingCompaniesHandler : IRequestHandler<GetAllShippingCompaniesQuery, GeneralResponse<List<ShippingCompanyDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllShippingCompaniesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<List<ShippingCompanyDto>>> Handle(GetAllShippingCompaniesQuery request, CancellationToken cancellationToken)
        {
            var query = await _unitOfWork.ShippingCompanyRepository.GetAllAsync();

            var result = query.Select(entity => new ShippingCompanyDto
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
            }).ToList();

            return GeneralResponse<List<ShippingCompanyDto>>.SuccessResponse("All shipping companies fetched successfully", result);
        }
    }
}
