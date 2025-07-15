using ShipConnect.DTOs.RatingDTOs;

namespace ShipConnect.CQRS.Ratings.Queries
{
    public class CompanyRatesQuery:IRequest<GeneralResponse<GetDataResult<List<CompanyRatesDTO>>>>
    {
        public int CompanyId { get;}
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public CompanyRatesQuery(int companyId, int pageNumber, int pageSize)
        {
            CompanyId = companyId;
            PageNumber = pageNumber;
            PageSize = pageSize;            
        }
    }

    public class CompanyRatesQueryHandler:IRequestHandler<CompanyRatesQuery, GeneralResponse<GetDataResult<List<CompanyRatesDTO>>>>
    {
        public IUnitOfWork _unitOfWork { get; }

        public CompanyRatesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<GetDataResult<List<CompanyRatesDTO>>>> Handle(CompanyRatesQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.RatingRepository.GetWithFilterAsync(r => r.ShippingCompany.Id == request.CompanyId);

            var total = await query.CountAsync(cancellationToken);

            if (total == 0) 
                return GeneralResponse<GetDataResult<List<CompanyRatesDTO>>>.FailResponse("No ratings for this company");



            var data =await query.OrderByDescending(r => r.CreatedAt)
                                .Skip((request.PageNumber - 1) * request.PageSize)
                                .Take(request.PageSize)
                                .Select(o => new CompanyRatesDTO
                                {
                                    StartUpName = o.StartUp.CompanyName,
                                    ImageUrl = o.StartUp.User.ProfileImageUrl,
                                    Score = o.Score,
                                    Comment=o.Comment ?? "N/A",
                                    RatedAt = o.CreatedAt,  
                                    ShipmentCode = o.Offer.Shipment.Code
                                }).ToListAsync(cancellationToken);

            var result = new GetDataResult<List<CompanyRatesDTO>>
            {
                Data = data,
                PageNumber =request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = total,
            };

            return GeneralResponse<GetDataResult<List<CompanyRatesDTO>>>.SuccessResponse("Company rating retrieved successfully", result);
        }
    }
}
