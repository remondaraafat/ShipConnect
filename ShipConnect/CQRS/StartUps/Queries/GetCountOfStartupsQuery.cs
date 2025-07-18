namespace ShipConnect.CQRS.StartUps.Queries
{
    public class GetCountOfStartupsQuery:IRequest<int>
    {
    }
    public class GetCountOfStartupsQueryHandler : IRequestHandler<GetCountOfStartupsQuery,int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetCountOfStartupsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(GetCountOfStartupsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.StartUpRepository.CountAsync(s=>s.User.IsApproved && s.CompanyName.ToUpper()!="ADMIN");
        }
    }
}
