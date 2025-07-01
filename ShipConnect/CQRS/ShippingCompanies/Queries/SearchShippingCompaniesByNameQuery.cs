using MediatR;
using ShipConnect.DTOs.ShippingCompanies;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.ShippingCompanies.Queries
{
    public class SearchShippingCompaniesByNameQuery : IRequest<GeneralResponse<List<ShippingCompanyDto>>>
    {
        public string Name { get; set; }

        public SearchShippingCompaniesByNameQuery(string name)
        {
            Name = name;
        }
    }

    public class SearchShippingCompaniesByNameHandler
        : IRequestHandler<SearchShippingCompaniesByNameQuery, GeneralResponse<List<ShippingCompanyDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchShippingCompaniesByNameHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<List<ShippingCompanyDto>>> Handle(SearchShippingCompaniesByNameQuery request, CancellationToken cancellationToken)
        {
            var companies = await _unitOfWork.ShippingCompanyRepository
                .GetWithFilterAsync(c => c.CompanyName.ToLower().Contains(request.Name.ToLower()));

            var result = companies.Select(c => new ShippingCompanyDto
            {
                Id = c.Id,
                CompanyName = c.CompanyName,
                City = c.City,
                Phone = c.Phone,
                Website = c.Website,
                Address = c.Address,
                Description = c.Description,
                LicenseNumber = c.LicenseNumber,
                TaxId = c.TaxId,
                UserId = c.UserId,
                TransportType = c.TransportType,
                ShippingScope = c.ShippingScope
            }).ToList();

            return GeneralResponse<List<ShippingCompanyDto>>.SuccessResponse("Matching shipping companies retrieved", result);
        }
    }
}
