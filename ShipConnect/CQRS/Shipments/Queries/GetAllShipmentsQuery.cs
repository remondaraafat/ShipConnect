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
            
            //var totalCount = query.

            
        }
    }

}
