


namespace ShipConnect.CQRS.StartUps.Queries
{
    public class GetStartupByEmailQuery : IRequest<GetStartupByEmailDTO>
    {
        public string Email { get; set; }
    }
    public class GetStartupByEmailQueryHandler : IRequestHandler<GetStartupByEmailQuery, GetStartupByEmailDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetStartupByEmailQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetStartupByEmailDTO> Handle(GetStartupByEmailQuery request, CancellationToken cancellationToken)
        {
           // var query = await _unitOfWork.StartUpRepository.GetWithFilterAsync(s => s.User.Email == request.Email);
            return _unitOfWork.StartUpRepository.GetWithFilterAsync(s => s.User.Email == request.Email).Select(s => new GetStartupByEmailDTO
            {
                Email = s.User.Email,
                Address = s.Address,
                BusinessCategory = s.BusinessCategory,
                Description = s.Description,
                Phone = s.User.PhoneNumber,
                StartupName = s.User.UserName
            }).FirstOrDefault();
        }
    }
}
