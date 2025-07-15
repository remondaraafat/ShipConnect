using Org.BouncyCastle.Ocsp;

namespace ShipConnect.CQRS.StartUps.Queries
{
    public class GetAllStartupsQuery : IRequest<PagedResult<GetAllUsersDTO>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }

    public class GetAllStartupsQueryHandler : IRequestHandler<GetAllStartupsQuery, PagedResult<GetAllUsersDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllStartupsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<GetAllUsersDTO>> Handle(GetAllStartupsQuery request, CancellationToken cancellationToken)
        {
            int total = await _unitOfWork.StartUpRepository.GetAllAsync().AsNoTracking().CountAsync(s => s.User.IsApproved && s.CompanyName.ToLower() != "ADMIN");
            
              List<GetAllUsersDTO> DTO =  await _unitOfWork.StartUpRepository.GetWithFilterAsync(s => s.User.IsApproved && s.CompanyName.ToLower() != "ADMIN")
                .OrderBy(s => s.Id) 
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(s => new GetAllUsersDTO
                {
                    Id = s.Id,
                    CompanyName = s.CompanyName, 
                    ProfileImageUrl = s.User.ProfileImageUrl,
                    AccountType = "Startup",
                    RegisterAt = s.CreatedAt,

                }).ToListAsync(cancellationToken);

            return new PagedResult<GetAllUsersDTO>
            {
                Items = DTO,
                TotalCount = total,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };
        }
    }
}
