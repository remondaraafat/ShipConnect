using MediatR;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;
using static ShipConnect.Enums.Enums;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class AllShipmentsCountQuery:IRequest<GeneralResponse<int>>  
    {
        public int? ShipmentStatus { get; set; }
    }

    public class AllShipmentsCountQueryHandler : IRequestHandler<AllShipmentsCountQuery, GeneralResponse<int>>
    {
        private readonly IUnitOfWork UnitOfWork;

        public AllShipmentsCountQueryHandler(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<int>> Handle(AllShipmentsCountQuery request, CancellationToken cancellationToken)
        {
            int ShipmentCount = -1;
            if (Enum.IsDefined(typeof(ShipmentStatus), request.ShipmentStatus))//بيتاكد هل القيمة موجودة فعليا ولا
            {
                ShipmentStatus status = (ShipmentStatus)request.ShipmentStatus;

                ShipmentCount = await UnitOfWork.ShipmentRepository.CountAsync(s => s.Status == status);
                return GeneralResponse<int>.SuccessResponse($"{status} Shipment Count", ShipmentCount);
            }

            ShipmentCount = await UnitOfWork.ShipmentRepository.CountAsync();
            return GeneralResponse<int>.SuccessResponse("Total Shipment Count", ShipmentCount);
        }
    }
}
