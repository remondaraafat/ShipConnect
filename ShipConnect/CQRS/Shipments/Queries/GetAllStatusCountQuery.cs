using ShipConnect.DTOs.ShipmentDTOs;
using ShipConnect.Models;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class GetAllStatusCountQuery : IRequest<GeneralResponse<GetAllStatusCountDTO>>
    {
        public string UserId { get; }

        public GetAllStatusCountQuery(string userId)=>UserId = userId;
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

            //بعمل كدة عشان الاستعلام يتنفذ مرة واحدة بس ف الداتا بيز
            var grouped = await query.GroupBy(s => 1) // يتأكد أننا نرجع صفًّا واحدًا
                                        .Select(g => new
                                        {
                                            Total = g.Count(),
                                            Pending = g.Count(s => s.Status == ShipmentStatus.Pending),
                                            Preparing = g.Count(s => s.Status == ShipmentStatus.Preparing),
                                            InTransit = g.Count(s => s.Status == ShipmentStatus.InTransit),
                                            AtWarehouse = g.Count(s => s.Status == ShipmentStatus.AtWarehouse),
                                            OutForDelivery = g.Count(s => s.Status == ShipmentStatus.OutForDelivery),
                                            Delivered = g.Count(s => s.Status == ShipmentStatus.Delivered),
                                            Failed = g.Count(s => s.Status == ShipmentStatus.Failed)
                                        }).FirstOrDefaultAsync(cancellationToken);

            if (grouped==null||grouped.Total == 0)
                return GeneralResponse<GetAllStatusCountDTO>.FailResponse("You don't have any shipments");

            var data = new GetAllStatusCountDTO
            {
                TotalCount = grouped.Total,
                Pending = grouped.Pending,
                Preparing = grouped.Preparing,
                InTransit = grouped.InTransit,
                AtWarehouse = grouped.AtWarehouse,
                OutForDelivery = grouped.OutForDelivery,
                Delivered = grouped.Delivered,
                Failed = grouped.Failed
            };

            return GeneralResponse<GetAllStatusCountDTO>.SuccessResponse("All Shipments status count retrieved successfully",data);
        }
    }

}
