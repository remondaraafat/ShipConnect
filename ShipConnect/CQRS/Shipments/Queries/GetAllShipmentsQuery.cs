using System.Text.RegularExpressions;
using MediatR;
using ShipConnect.DTOs.ShipmentDTOs;
using ShipConnect.Helpers;
using ShipConnect.Models;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Shipments.Queries
{
    public class GetAllShipmentsQuery : IRequest<GeneralResponse<GetDataResult<List<GetAllShipmentsDTO>>>>
    {
        public string UserId { get;}
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllShipmentsQuery(string userId, int pageNumber, int pageSize)
        {
            UserId = userId;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
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
                    return GeneralResponse<GetDataResult<List<GetAllShipmentsDTO>>>.FailResponse("Shipping Company not found");

                query = UnitOfWork.OfferRepository.GetWithFilterAsync(o => o.ShippingCompanyId == company.Id && o.IsAccepted).Select(s => s.Shipment);

            }

            int totalCount = await query.CountAsync(cancellationToken);
            if (totalCount == 0)
                return GeneralResponse<GetDataResult<List<GetAllShipmentsDTO>>>.FailResponse("You didn't have any shipments");


            var shipments = await query.OrderByDescending(c => c.SentDate)
                                        .Skip((request.PageNumber - 1) * request.PageSize)
                                        .Take(request.PageSize)
                                        .Select(q => new GetAllShipmentsDTO
                                        {
                                            Id = q.Id,
                                            Code = q.Code,
                                            Status = Regex.Replace(q.Status.ToString(), "(\\B[A-Z])", " $1"),
                                            RequestedPickupDate = q.RequestedPickupDate,
                                        }).ToListAsync(cancellationToken);

            var dataResult = new GetDataResult<List<GetAllShipmentsDTO>>
            {
                Data = shipments,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

            return GeneralResponse<GetDataResult<List<GetAllShipmentsDTO>>>.SuccessResponse("Get All Shipment data retrieved successfuly", dataResult);
        }
    }

}