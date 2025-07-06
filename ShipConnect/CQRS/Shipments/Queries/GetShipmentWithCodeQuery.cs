using System.Linq;
using ShipConnect.DTOs.ShipmentDTOs;
using ShipConnect.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            IQueryable<Shipment> query;

            var startUp = await UnitOfWork.StartUpRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserId);
            if (startUp != null)
            {
                query = UnitOfWork.ShipmentRepository
                            .GetWithFilterAsync(sh => sh.StartupId == startUp.Id && sh.Code.Contains(request.Code));
            }
            else
            {
                var company = await UnitOfWork.ShippingCompanyRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserId);

                if (company == null)
                    return GeneralResponse<List<GetAllShipmentsDTO>>.FailResponse("User not found");

                query = UnitOfWork.OfferRepository
                        .GetWithFilterAsync(o => o.ShippingCompanyId == company.Id && o.IsAccepted)
                        .Select(o => o.Shipment!)
                        .Where(s => s.Code.Contains(request.Code));
            }
        
            
        var shipment = query.OrderByDescending(o=>o.RequestedPickupDate).ToList();
            
            if (!shipment.Any())
                return GeneralResponse<List<GetAllShipmentsDTO>>.FailResponse("No shipments found matching the code");


            var result = shipment.Select(q => new GetAllShipmentsDTO
            {
                Id = q.Id,
                Code = q.Code,
                Status = q.Status.ToString(),
                RequestedPickupDate = q.RequestedPickupDate,
            }).ToList();

            return GeneralResponse<List<GetAllShipmentsDTO>>.SuccessResponse("Get Shipment data matching with code retrieved successfuly", result);
        }
    }

}
