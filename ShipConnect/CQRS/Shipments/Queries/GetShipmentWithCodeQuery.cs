using System.Linq;
using ShipConnect.DTOs.ShipmentDTOs;
using ShipConnect.Models;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class GetShipmentWithCodeQuery:IRequest<GeneralResponse<List<GetAllShipmentsDTO>>>
    {
        public string UserId { get; set; }
        public string Code { get; set; }
    }

    public class GetShipmentWithCodeQueryHandler : IRequestHandler<GetShipmentWithCodeQuery, GeneralResponse<List<GetAllShipmentsDTO>>>
    {
        public IUnitOfWork UnitOfWork { get; }

        public GetShipmentWithCodeQueryHandler(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<List<GetAllShipmentsDTO>>> Handle(GetShipmentWithCodeQuery request, CancellationToken cancellationToken)
        {
            var startUp = await UnitOfWork.StartUpRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserId);

            if (startUp == null)
                return GeneralResponse<List<GetAllShipmentsDTO>>.FailResponse("Startup not found");

            var query = UnitOfWork.ShipmentRepository
                            .GetWithFilterAsync(sh => sh.StartupId == startUp.Id &&sh.Code.Contains(request.Code))
                            .OrderByDescending(o=>o.RequestedPickupDate).ToList();
            
            if (!query.Any())
                return GeneralResponse<List<GetAllShipmentsDTO>>.FailResponse("No shipments found matching the code");


            var shipment = query.Select(q => new GetAllShipmentsDTO
            {
                Id = q.Id,
                Code = q.Code,
                Status = q.Status.ToString(),
                RequestedPickupDate = q.RequestedPickupDate,
            }).ToList();

            return GeneralResponse<List<GetAllShipmentsDTO>>.SuccessResponse("Get Shipment data matching with code retrieved successfuly", shipment);
        }
    }

}
