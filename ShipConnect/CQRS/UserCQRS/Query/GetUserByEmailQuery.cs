using ShipConnect.DTOs.UserDTOs;

namespace ShipConnect.CQRS.AdminProfileCQRS.Query
{
    public class GetUserByEmailQuery : IRequest<GetUserDTO?>
    {
        public string Email { get; set; } = default!;
    }
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, GetUserDTO?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByEmailQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetUserDTO?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.ApplicationUserRepository
                .GetFirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                return null;

            return new GetUserDTO
            {
                Name = user.Name ?? string.Empty,
                Phone = user.PhoneNumber ?? string.Empty,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl ?? string.Empty
            };
        }
    }
}
