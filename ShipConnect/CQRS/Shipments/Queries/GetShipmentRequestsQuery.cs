using ShipConnect.DTOs.ShipmentDTOs;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class GetShipmentRequestsQuery:IRequest<GeneralResponse<GetDataResult<List<ShipmentRequestsDTO>>>>
    {
        public string UserId { get;}
        public int PageNumber { get; }
        public int PageSize { get; }

        public GetShipmentRequestsQuery(string userId, int pageNumber, int pageSize)
        {
            UserId = userId;            
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class GetShipmentRequestsQueryHandler : IRequestHandler<GetShipmentRequestsQuery, GeneralResponse<GetDataResult<List<ShipmentRequestsDTO>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetShipmentRequestsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;            
        }

        public async Task<GeneralResponse<GetDataResult<List<ShipmentRequestsDTO>>>> Handle(GetShipmentRequestsQuery request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.ShippingCompanyRepository.GetFirstOrDefaultAsync(c => c.UserId == request.UserId);
            if (company == null)
                return GeneralResponse<GetDataResult<List<ShipmentRequestsDTO>>>.FailResponse("User not found");

            var requests = await _unitOfWork.ShipmentRepository
                                            .GetWithFilterAsync(s=>s.Status==ShipmentStatus.Pending
                                            &&!s.Offers.Any(o=>o.IsAccepted))
                                            .OrderByDescending(r=>r.CreatedAt)
                                            .Skip((request.PageNumber - 1) * request.PageSize)
                                            .Take(request.PageSize)
                                            .Select(s => new ShipmentRequestsDTO
                                            {
                                                ShipmentId=s.Id,
                                                StartupName=s.Startup.CompanyName,
                                                ShipmentType=s.ShipmentType,
                                                SentDate = s.SentDate,
                                                SenderAddress=s.SenderAddress,
                                                DestinationAddress=s.DestinationAddress
                                        }).ToListAsync(cancellationToken);

            var dataResult = new GetDataResult<List<ShipmentRequestsDTO>>
            {
                Data = requests,
                TotalCount = await _unitOfWork.ShipmentRepository.CountAsync(s => s.Status == ShipmentStatus.Pending && !s.Offers.Any(o => o.IsAccepted)),
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

            if(!requests.Any())
                return GeneralResponse<GetDataResult<List<ShipmentRequestsDTO>>>.FailResponse("No available shipments at the moment");

            return GeneralResponse<GetDataResult<List<ShipmentRequestsDTO>>>.SuccessResponse("Available shipments retrieved successfully", dataResult);
        }
    }

}
