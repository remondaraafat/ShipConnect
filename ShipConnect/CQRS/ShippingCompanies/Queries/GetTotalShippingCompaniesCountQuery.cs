using MediatR;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.ShippingCompanies.Queries
{
    public class GetTotalShippingCompaniesCountQuery : IRequest<GeneralResponse<int>>
    {
    }

    public class GetTotalShippingCompaniesCountHandler : IRequestHandler<GetTotalShippingCompaniesCountQuery, GeneralResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTotalShippingCompaniesCountHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<int>> Handle(GetTotalShippingCompaniesCountQuery request, CancellationToken cancellationToken)
        {
            var companies = await _unitOfWork.ShippingCompanyRepository.GetAllAsync();
            int count = companies.Count();

            return GeneralResponse<int>.SuccessResponse("Total shipping companies count retrieved successfully", count);
        }
    }
}
