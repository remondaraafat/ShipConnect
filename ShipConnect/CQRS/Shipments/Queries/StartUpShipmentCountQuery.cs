using MediatR;
using ShipConnect.CQRS.Shipments.Commands;
using ShipConnect.Helpers;
using ShipConnect.Models;
using ShipConnect.UnitOfWorkContract;
using static ShipConnect.Enums.Enums;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class StartUpShipmentStatusCountQuery : IRequest<GeneralResponse<int>>
    {
        public string UserId { get; set; }
        public int? ShipmentStatus { get; set; }
    }

    public class StartUpShipmentStatusCountQuereyHandler : IRequestHandler<StartUpShipmentStatusCountQuery, GeneralResponse<int>>
    {
        private readonly IUnitOfWork UnitOfWork;

        public StartUpShipmentStatusCountQuereyHandler(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<int>> Handle(StartUpShipmentStatusCountQuery request, CancellationToken cancellationToken)
        {
            var startUp = await UnitOfWork.StartUpRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserId);
            if (startUp == null) 
                return GeneralResponse<int>.FailResponse("Startup not found for current user");

            int ShipmentCount = -1;
            if(Enum.IsDefined(typeof(ShipmentStatus),request.ShipmentStatus))//بيتاكد هل القيمة موجودة فعليا ولا
            {
                ShipmentStatus status = (ShipmentStatus)request.ShipmentStatus;

                ShipmentCount = await UnitOfWork.ShipmentRepository.CountAsync(s => s.Status == status && s.StartupId == startUp.Id);
                return GeneralResponse<int>.SuccessResponse($"{status} Shipment Count", ShipmentCount);
            }
                
            ShipmentCount = await UnitOfWork.ShipmentRepository.CountAsync(s => s.StartupId == startUp.Id);
            return GeneralResponse<int>.SuccessResponse("Total Shipment Count", ShipmentCount);
        }
    }
}
