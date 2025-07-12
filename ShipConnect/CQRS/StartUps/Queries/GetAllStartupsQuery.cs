using Org.BouncyCastle.Ocsp;

namespace ShipConnect.CQRS.StartUps.Queries
{
    public class GetAllStartupsQuery:IRequest<PagedResult<GetAllStartupsDTO>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public class GetAllStartupsQueryHandler : IRequestHandler<GetAllStartupsQuery,PagedResult<GetAllStartupsDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllStartupsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PagedResult<GetAllStartupsDTO>> Handle(GetAllStartupsQuery request, CancellationToken cancellationToken)
        {
            int total =await _unitOfWork.StartUpRepository.GetAllAsync().AsNoTracking().CountAsync();

            
            List<GetAllStartupsDTO> DTO =  await _unitOfWork.StartUpRepository.GetAllAsync()
                .OrderBy(s => s.Id) 
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(s => new GetAllStartupsDTO
                {
                    Address = s.Address,
                    BusinessCategory = s.BusinessCategory,
                    Description = s.Description,
                    Email = s.User.Email,
                    Phone = s.User.PhoneNumber,
                    ProfileImageUrl = s.User.ProfileImageUrl ?? string.Empty,
                    StartupName = s.User.Name,
                    Website = s.Website,
                    TaxId = s.TaxId
                }).ToListAsync(cancellationToken);

            return new PagedResult<GetAllStartupsDTO>
            {
                Items = DTO,
                TotalCount = total,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
        }
    }
}
