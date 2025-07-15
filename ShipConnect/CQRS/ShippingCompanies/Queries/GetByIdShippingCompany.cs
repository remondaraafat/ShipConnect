using MediatR;
using ShipConnect.DTOs.ShippingCompanies;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.ShippingCompanies.Querys
{
    public class GetShippingCompanyByIdQuery : IRequest<GeneralResponse<ShippingCompanyDto>>
    {
        public int? CompanyId { get; }
        public string? UserId { get; }

        public GetShippingCompanyByIdQuery(int? companyId, string? userId)
        {
            CompanyId = companyId;
            UserId = userId;
        }
    }

    public class GetShippingCompanyByIdHandler : IRequestHandler<GetShippingCompanyByIdQuery, GeneralResponse<ShippingCompanyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetShippingCompanyByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<ShippingCompanyDto>> Handle(GetShippingCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.ShippingCompanyRepository.GetAllAsync();

            if (request.CompanyId != null)
                query = query.Where(s => s.Id == request.CompanyId);

            else if (!string.IsNullOrEmpty(request.UserId))
                query = query.Where(s => s.UserId == request.UserId);

            else
                return GeneralResponse<ShippingCompanyDto>.FailResponse("No identifier provided");


            var data = await query.Select(s=> new ShippingCompanyDto
            {
                Id = s.Id,
                Address= s.Address ?? "N/A",
                CompanyName = s.CompanyName,
                Description = s.Description ?? "N/A",
                Email = s.User.Email ?? "N/A",
                LicenseNumber = s.LicenseNumber ?? "N/A",
                Phone = s.Phone ?? "N/A",    
                ProfileImageUrl = s.User.ProfileImageUrl,
                ShippingScope = s.ShippingScope,
                TransportType = s.TransportType,
                TaxId = s.TaxId ?? "N/A",
                Website = s.Website ?? "N/A"
            }).FirstOrDefaultAsync(cancellationToken);

            if (data == null)
                return GeneralResponse<ShippingCompanyDto>.FailResponse("company not Found");

            return GeneralResponse<ShippingCompanyDto>.SuccessResponse("Shipping Company data retrieved successfully", data);
        }
    }
}
