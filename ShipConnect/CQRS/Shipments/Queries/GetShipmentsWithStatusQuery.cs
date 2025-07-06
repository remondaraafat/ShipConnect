using ShipConnect.DTOs.ShipmentDTOs;
using ShipConnect.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class GetShipmentsWithStatusQuery:IRequest<GeneralResponse<GetDataResult<List<GetAllShipmentsDTO>>>>
    {
        public string UserId { get; set; }
        public int Status { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetShipmentsWithStatusQueryHandler : IRequestHandler<GetShipmentsWithStatusQuery, GeneralResponse<GetDataResult<List<GetAllShipmentsDTO>>>>
    {
        public IUnitOfWork UnitOfWork { get; }

        public GetShipmentsWithStatusQueryHandler(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<GetDataResult<List<GetAllShipmentsDTO>>>> Handle(GetShipmentsWithStatusQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Shipment> query;

            if (!Enum.IsDefined(typeof(ShipmentStatus), request.Status))//بيتاكد هل القيمة موجودة فعليا ولا
                return GeneralResponse<GetDataResult<List<GetAllShipmentsDTO>>>.FailResponse("Invalid shipment status");

            ShipmentStatus shipmentStatus = (ShipmentStatus)request.Status;
            
            var startUp = await UnitOfWork.StartUpRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserId);
            if (startUp != null)
            {
                query = UnitOfWork.ShipmentRepository.GetWithFilterAsync(sh => sh.StartupId == startUp.Id && sh.Status == shipmentStatus);
            }
            else
            {
                var company = await UnitOfWork.ShippingCompanyRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserId);
                if (company == null)
                    return GeneralResponse<GetDataResult<List<GetAllShipmentsDTO>>>.FailResponse("Shipping Company not found");

                query = UnitOfWork.OfferRepository.GetWithFilterAsync(o => o.ShippingCompanyId == company.Id && o.IsAccepted == true)
                                                    .Select(s => s.Shipment!)
                                                    .Where(s => s.Status == shipmentStatus);
            }

            int totalCount = query.Count();
            if (totalCount == 0)
                return GeneralResponse<GetDataResult<List<GetAllShipmentsDTO>>>.FailResponse("No shipments with selected status");

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

            return GeneralResponse<GetDataResult<List<GetAllShipmentsDTO>>>.SuccessResponse(
                $"{shipmentStatus} shipments retrieved successfully", dataResult);

        }
    }
}
