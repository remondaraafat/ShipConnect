using ShipConnect.DTOs.ShipmentDTOs;
using ShipConnect.Models;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class GetAllStatusCountQuery : IRequest<GeneralResponse<GetAllStatusCountDTO>>
    {
        public string UserId { get; set; }
    }

    public class GetAllStatusCountQueryHandler : IRequestHandler<GetAllStatusCountQuery, GeneralResponse<GetAllStatusCountDTO>>
    {
        public IUnitOfWork UnitOfWork { get; }

        public GetAllStatusCountQueryHandler(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<GetAllStatusCountDTO>> Handle(GetAllStatusCountQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Shipment> query;

            var startUp = await UnitOfWork.StartUpRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserId);
            if (startUp != null)
            {
                query = UnitOfWork.ShipmentRepository.GetWithFilterAsync(sh => sh.StartupId == startUp.Id);
            }
            else
            {
                var company = await UnitOfWork.ShippingCompanyRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserId);
                if (company == null)
                    return GeneralResponse<GetAllStatusCountDTO>.FailResponse("User not found");

                query = UnitOfWork.OfferRepository.GetWithFilterAsync(o => o.ShippingCompanyId == company.Id && o.IsAccepted == true).Select(s => s.Shipment!);

            }

            int totalCount = query.Count();
            if (totalCount == 0)
                return GeneralResponse<GetAllStatusCountDTO>.FailResponse("You don't have any shipments");

            var data = new GetAllStatusCountDTO
            {
                Pending = query.Count(c => c.Status == ShipmentStatus.Pending),
                Preparing = query.Count(c => c.Status == ShipmentStatus.Preparing),
                InTransit = query.Count(c => c.Status == ShipmentStatus.InTransit),
                AtWarehouse = query.Count(c => c.Status == ShipmentStatus.AtWarehouse),
                OutForDelivery = query.Count(c => c.Status == ShipmentStatus.OutForDelivery),
                Delivered = query.Count(c => c.Status == ShipmentStatus.Delivered),
                Failed = query.Count(c => c.Status == ShipmentStatus.Failed),
                TotalCount = totalCount
            };

            return GeneralResponse<GetAllStatusCountDTO>.SuccessResponse("All Shipments status count retrieved successfully",data);
        }
    }

}
