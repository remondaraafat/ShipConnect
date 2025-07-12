using MediatR;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;
namespace ShipConnect.CQRS.StartUps
{
    public class GetTotalStartUpCountQuery : IRequest<GeneralResponse<int>>
    {
    }

    public class GetTotalStartUpCountQueryHandler : IRequestHandler<GetTotalStartUpCountQuery, GeneralResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTotalStartUpCountQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<int>> Handle(GetTotalStartUpCountQuery request, CancellationToken cancellationToken)
        {
            int count = _unitOfWork.StartUpRepository.GetAllAsync().Count();

            return GeneralResponse<int>.SuccessResponse("Total Startups count retrieved successfully", count);
        }
    }
}
