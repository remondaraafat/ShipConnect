using ShipConnect.DTOs.ShippingCompanies;

namespace ShipConnect.ShippingCompanies.Querys
{
    public class GetAllShippingCompaniesQuery : IRequest<GeneralResponse<GetDataResult<List<GetAllUsersDTO>>>> 
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllShippingCompaniesQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class GetAllShippingCompaniesHandler : IRequestHandler<GetAllShippingCompaniesQuery, GeneralResponse<GetDataResult<List<GetAllUsersDTO>>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllShippingCompaniesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<GetDataResult<List<GetAllUsersDTO>>>> Handle(GetAllShippingCompaniesQuery request, CancellationToken cancellationToken)
        {
            int total = await _unitOfWork.ShippingCompanyRepository.CountAsync(s => s.User.IsApproved && s.CompanyName.ToUpper() != "ADMIN");
            if (total == 0)
                return GeneralResponse<GetDataResult<List<GetAllUsersDTO>>>.SuccessResponse("No shipping companies yet");

            var companies = await _unitOfWork.ShippingCompanyRepository
                                    .GetWithFilterAsync(c=>c.User.IsApproved && c.CompanyName.ToUpper() != "ADMIN")
                                    .OrderByDescending(c=>c.CreatedAt)
                                    .Skip((request.PageNumber-1)*request.PageSize)
                                    .Take(request.PageSize)
                                    .Select(entity => new GetAllUsersDTO
                                    {
                                        Id = entity.Id,
                                        ProfileImageUrl = entity.User.ProfileImageUrl,
                                        CompanyName = entity.CompanyName,
                                        AccountType = "Shipping Company",
                                        RegisterAt = entity.CreatedAt
                                    }).ToListAsync(cancellationToken);

            var dateResult = new GetDataResult<List<GetAllUsersDTO>>
            {
                Data = companies,
                TotalCount = total,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
            };

            return GeneralResponse<GetDataResult<List<GetAllUsersDTO>>>.SuccessResponse("All shipping companies fetched successfully", dateResult);
        }
    }
}
