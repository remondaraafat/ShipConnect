using ShipConnect.DTOs.ShippingCompanies;

namespace ShipConnect.ShippingCompanies.Querys
{
    public class GetAllShippingCompaniesQuery : IRequest<GeneralResponse<GetDataResult<List<ShippingCompanyDto>>>> 
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllShippingCompaniesQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class GetAllShippingCompaniesHandler : IRequestHandler<GetAllShippingCompaniesQuery, GeneralResponse<GetDataResult<List<ShippingCompanyDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllShippingCompaniesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<GetDataResult<List<ShippingCompanyDto>>>> Handle(GetAllShippingCompaniesQuery request, CancellationToken cancellationToken)
        {
            var companies = await _unitOfWork.ShippingCompanyRepository
                                    .GetAllAsync()
                                    .OrderByDescending(c=>c.CreatedAt)
                                    .Skip((request.PageNumber-1)*request.PageSize)
                                    .Take(request.PageSize)
                                    .Select(entity => new ShippingCompanyDto
                                    {
                                        Id = entity.Id,
                                        CompanyName = entity.CompanyName,
                                        ProfileImageUrl = entity.User.ProfileImageUrl,
                                        Description = entity.Description,
                                        Address = entity.Address,
                                        Phone = entity.Phone,
                                        Website = entity.Website,
                                        LicenseNumber = entity.LicenseNumber,
                                        UserId = entity.UserId,
                                        TransportType = entity.TransportType,
                                        ShippingScope = entity.ShippingScope,
                                        TaxId = entity.TaxId
                                    }).ToListAsync(cancellationToken);

            var dateResult = new GetDataResult<List<ShippingCompanyDto>>
            {
                Data = companies,
                TotalCount = await _unitOfWork.ShippingCompanyRepository.CountAsync(),
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
            };

            return GeneralResponse<GetDataResult<List<ShippingCompanyDto>>>.SuccessResponse("All shipping companies fetched successfully", dateResult);
        }
    }
}
