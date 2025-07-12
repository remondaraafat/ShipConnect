using System.Linq;
using System.Text.RegularExpressions;
using ShipConnect.DTOs.ShipmentDTOs;
using ShipConnect.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class GetShipmentWithCodeQuery:IRequest<GeneralResponse<List<GetAllShipmentsDTO>>>
    {
        public string UserId { get; }
        public string Code { get; set; }

        public GetShipmentWithCodeQuery(string userId, string code)
        {
            UserId = userId;
            Code = code;
        }
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
            if (string.IsNullOrWhiteSpace(request.Code))
                return GeneralResponse<List<GetAllShipmentsDTO>>.FailResponse("Code cannot be empty");

            IQueryable<Shipment> query;

            var startUp = await UnitOfWork.StartUpRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserId);
            if (startUp != null)
            {
                query = UnitOfWork.ShipmentRepository
                            .GetWithFilterAsync(sh => sh.StartupId == startUp.Id && sh.Code.ToLower().Contains(request.Code.ToLower()));
            }
            else
            {
                var company = await UnitOfWork.ShippingCompanyRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserId);

                if (company == null)
                    return GeneralResponse<List<GetAllShipmentsDTO>>.FailResponse("User not found");

                query = UnitOfWork.OfferRepository
                        .GetWithFilterAsync(o => o.ShippingCompanyId == company.Id && o.IsAccepted)
                        .Select(o => o.Shipment!)
                        .Where(s => s.Code.ToLower().Contains(request.Code.ToLower()));
            }


            var result = await query.OrderByDescending(s => s.RequestedPickupDate)
                .Select(q => new GetAllShipmentsDTO
                {
                    Id = q.Id,
                    Code = q.Code,
                    Status = Regex.Replace(q.Status.ToString(), "(\\B[A-Z])", " $1"),
                    RequestedPickupDate = q.RequestedPickupDate
                })
                .ToListAsync(cancellationToken);

            if (!result.Any())
                return GeneralResponse<List<GetAllShipmentsDTO>>.FailResponse("No shipments found matching the code");

            return GeneralResponse<List<GetAllShipmentsDTO>>.SuccessResponse("Get Shipment data matching with code retrieved successfuly", result);
        }
    }

}
