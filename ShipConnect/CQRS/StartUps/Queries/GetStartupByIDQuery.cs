using ShipConnect.DTOs.StartUpDTOs;
using Microsoft.EntityFrameworkCore;
namespace ShipConnect.CQRS.StartUps.Queries
{
    public class GetStartupByIDQuery:IRequest<GetStartupByIdDTO>
    {
        public string Id { get; set; }
    }
    public class GetStartupByIDQueryHandler : IRequestHandler<GetStartupByIDQuery, GetStartupByIdDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetStartupByIDQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetStartupByIdDTO> Handle(GetStartupByIDQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.StartUpRepository.GetWithFilterAsync(s => s.UserId == request.Id)
                .Select(s => new GetStartupByIdDTO
                {
                    Email = s.User.Email,
                    Address = s.Address,
                    ProfileImageUrl = s.User.ProfileImageUrl ?? string.Empty,
                    BusinessCategory = s.BusinessCategory,
                    Description = s.Description,
                    Phone = s.User.PhoneNumber,
                    StartupName = s.User.Name,
                    Website = s.Website,
                    TaxId = s.TaxId
                }).FirstOrDefaultAsync();
        }
    }
}
