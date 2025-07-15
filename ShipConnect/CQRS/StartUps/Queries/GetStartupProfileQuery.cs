namespace ShipConnect.CQRS.StartUps.Queries
{
    public class GetStartupProfileQuery : IRequest<GeneralResponse<GetStartupProfileDTO>>
    {
        public int? StartUpId { get;}
        public string? UserId {get;}

        public GetStartupProfileQuery(int? startUpId, string? userId)
        {
            StartUpId = startUpId;
            UserId = userId;           
        }

    }
    public class GetStartupProfileQueryHandler : IRequestHandler<GetStartupProfileQuery, GeneralResponse<GetStartupProfileDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetStartupProfileQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GeneralResponse<GetStartupProfileDTO>> Handle(GetStartupProfileQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.StartUpRepository.GetAllAsync();

            if (request.StartUpId != null)
                query = query.Where(s => s.Id == request.StartUpId);

            else if (!string.IsNullOrEmpty(request.UserId))
                query = query.Where(s => s.UserId == request.UserId);
           
            else
                return GeneralResponse<GetStartupProfileDTO>.FailResponse("No identifier provided");

            var data = await query.Select(s=>new GetStartupProfileDTO
                                {
                                    Id = s.Id,
                                    Address = s.Address ?? "N/A",
                                    BusinessCategory = s.BusinessCategory ?? "N/A",
                                    Description = s.Description ?? "N/A",
                                    Email = s.User.Email ?? "N/A",
                                    Phone = s.Phone ?? "N/A",
                                    ProfileImageUrl = s.User.ProfileImageUrl,
                                    StartupName = s.CompanyName,
                                    TaxId = s.TaxId ?? "N/A"
                                }).FirstOrDefaultAsync(cancellationToken);

                if (data == null)
                    return GeneralResponse<GetStartupProfileDTO>.FailResponse("company not Found");

            return GeneralResponse<GetStartupProfileDTO>.SuccessResponse("StartUp data retrieved successfully",data);
        }
    }
}
