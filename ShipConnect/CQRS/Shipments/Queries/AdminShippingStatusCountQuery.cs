using ShipConnect.DTOs.ShipmentDTOs;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class AdminShippingStatusCountQuery : IRequest<GeneralResponse<GetAllStatusCountDTO>>
    {
        public int ShippingCompanyID { get; set; }
    }

    public class AdminShippingStatusCountQueryHandler : IRequestHandler<AdminShippingStatusCountQuery, GeneralResponse<GetAllStatusCountDTO>>
    {
        public IUnitOfWork UnitOfWork { get; }

        public AdminShippingStatusCountQueryHandler(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<GetAllStatusCountDTO>> Handle(AdminShippingStatusCountQuery request, CancellationToken cancellationToken)
        {
            var company = await UnitOfWork.ShippingCompanyRepository.GetFirstOrDefaultAsync(s => s.Id == request.ShippingCompanyID);

            if (company == null)
                return GeneralResponse<GetAllStatusCountDTO>.FailResponse("Shipping Company not found");

            var Shipments = UnitOfWork.OfferRepository.GetWithFilterAsync(o => o.ShippingCompanyId == company.Id && o.IsAccepted == true).Select(s => s.Shipment);
            if (Shipments == null)
                return GeneralResponse<GetAllStatusCountDTO>.FailResponse("No shipments found for this company");

            var data = new GetAllStatusCountDTO
            {
                Pending = Shipments.Count(c => c.Status == ShipmentStatus.Pending),
                Preparing = Shipments.Count(c => c.Status == ShipmentStatus.Preparing),
                InTransit = Shipments.Count(c => c.Status == ShipmentStatus.InTransit),
                AtWarehouse = Shipments.Count(c => c.Status == ShipmentStatus.AtWarehouse),
                OutForDelivery = Shipments.Count(c => c.Status == ShipmentStatus.OutForDelivery),
                Delivered = Shipments.Count(c => c.Status == ShipmentStatus.Delivered),
                Failed = Shipments.Count(c => c.Status == ShipmentStatus.Failed),
                TotalCount = Shipments.Count()
            };

            return GeneralResponse<GetAllStatusCountDTO>.SuccessResponse("All Shipments status count retrieved successfully", data);
        }
    }
}
