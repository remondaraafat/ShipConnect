using ShipConnect.ShippingCompanies.Querys;

namespace ShipConnect.CQRS.UserCQRS.Query
{
    public class GetAllUsersQuery : IRequest<GeneralResponse<GetDataResult<List<GetAllUsersDTO>>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllUsersQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, GeneralResponse<GetDataResult<List<GetAllUsersDTO>>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllUsersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<GetDataResult<List<GetAllUsersDTO>>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var startUps = _unitOfWork.StartUpRepository.GetWithFilterAsync(s=>s.User.IsApproved&&s.User.Name!="Admin")
                                        .Select(s => new GetAllUsersDTO
                                        {
                                            Id = s.Id,
                                            ProfileImageUrl = s.User.ProfileImageUrl,
                                            CompanyName = s.CompanyName,
                                            AccountType = "Startup",
                                            RegisterAt = s.CreatedAt,
                                        });

            var companies = _unitOfWork.ShippingCompanyRepository.GetWithFilterAsync(c => c.User.IsApproved && c.User.Name != "Admin")
                                        .Select(entity => new GetAllUsersDTO
                                        {
                                            Id = entity.Id,
                                            ProfileImageUrl = entity.User.ProfileImageUrl,
                                            CompanyName = entity.CompanyName,
                                            AccountType = "Shipping Company",
                                            RegisterAt = entity.CreatedAt
                                        });

            var users = startUps.Concat(companies);
            var total = await users.CountAsync(cancellationToken);

            var data = await users.OrderByDescending(u => u.RegisterAt)
                                    .Skip((request.PageNumber - 1) * request.PageSize)
                                    .Take(request.PageSize)
                                    .ToListAsync(cancellationToken);

            var result = new GetDataResult<List<GetAllUsersDTO>>
            {
                Data = data,
                TotalCount = total,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
            };

            return GeneralResponse<GetDataResult<List<GetAllUsersDTO>>>.SuccessResponse("Users retrieved successfully", result);
        }
    }
}
