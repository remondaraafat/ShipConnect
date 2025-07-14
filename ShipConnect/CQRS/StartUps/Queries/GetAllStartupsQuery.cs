using Org.BouncyCastle.Ocsp;

namespace ShipConnect.CQRS.StartUps.Queries
{
    public class GetAllStartupsQuery:IRequest<GeneralResponse<GetDataResult<List<GetAllUsersDTO>>>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public GetAllStartupsQuery(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;            
        }
    }
    public class GetAllStartupsQueryHandler : IRequestHandler<GetAllStartupsQuery,GeneralResponse<GetDataResult<List<GetAllUsersDTO>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllStartupsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<GetDataResult<List<GetAllUsersDTO>>>> Handle(GetAllStartupsQuery request, CancellationToken cancellationToken)
        {
            int total =await _unitOfWork.StartUpRepository.CountAsync(s=>s.User.IsApproved && s.User.Name != "Admin");
            if(total==0)
                return GeneralResponse<GetDataResult<List<GetAllUsersDTO>>>.FailResponse("No startups yet");

            var DTO =  await _unitOfWork.StartUpRepository.GetWithFilterAsync(s=>s.User.IsApproved && s.User.Name != "Admin")
                                        .OrderByDescending(s => s.CreatedAt) 
                                        .Skip((request.PageIndex - 1) * request.PageSize)
                                        .Take(request.PageSize)
                                        .Select(s => new GetAllUsersDTO
                                        {
                                            Id = s.Id,
                                            ProfileImageUrl = s.User.ProfileImageUrl ?? string.Empty,
                                            CompanyName = s.CompanyName,
                                            AccountType = "Startup",
                                            RegisterAt = s.CreatedAt,
                                        }).ToListAsync(cancellationToken);

            var data =  new GetDataResult<List<GetAllUsersDTO>>
            {
                Data = DTO,
                TotalCount = total,
                PageNumber = request.PageIndex,
                PageSize = request.PageSize
            };

            return GeneralResponse<GetDataResult<List<GetAllUsersDTO>>>.SuccessResponse("All Startups retrieved successfully", data);
        }
    }
}
