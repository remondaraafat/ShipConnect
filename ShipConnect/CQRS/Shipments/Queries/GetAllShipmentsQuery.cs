using MediatR;
using ShipConnect.DTOs.ShipmentDTOs;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class GetAllShipmentsQuery:IRequest<GeneralResponse<GetDataResult<List<GetAllShipmentsDTO>>>>
    {
        public string UserId { get; set; }
        public int PageNumber {  get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllShipmentsQueryHandler : IRequestHandler<GetAllShipmentsQuery, GeneralResponse<GetDataResult<List<GetAllShipmentsDTO>>>>
    {
        public IUnitOfWork UnitOfWork { get; }

        public GetAllShipmentsQueryHandler(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<GetDataResult<List<GetAllShipmentsDTO>>>> Handle(GetAllShipmentsQuery request, CancellationToken cancellationToken)
        {
            var startUp = await UnitOfWork.StartUpRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserId);

            if (startUp == null)
                return GeneralResponse<GetDataResult<List<GetAllShipmentsDTO>>>.FailResponse("Startup not found");

            var query = UnitOfWork.ShipmentRepository.GetWithFilterAsync(sh => sh.StartupId == startUp.Id);
                                                    
            int totalCount = query.Count();

            var shipments = query.OrderByDescending(c => c.SentDate)
                                                    .Skip((request.PageNumber - 1) * request.PageSize)
                                                    .Take(request.PageSize)
                                                    .ToList();

            var result = shipments.Select(q => new GetAllShipmentsDTO
            {
                Id = q.Id,
                Code = q.Code,
                Status = q.Status.ToString(),
                RequestedPickupDate = q.RequestedPickupDate,
            }).ToList();

            var dataResult = new GetDataResult<List<GetAllShipmentsDTO>>
            {
                Data = result,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

            return GeneralResponse<GetDataResult<List<GetAllShipmentsDTO>>>.SuccessResponse("Get All Shipment data retrieved successfuly",dataResult);
        }
    }

}
