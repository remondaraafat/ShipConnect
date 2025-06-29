using MediatR;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.ShippingCompanies.Queries
{
    public class GetTotalShippingCompaniesCountQuery : IRequest<int>
    {
    }

    public class GetTotalShippingCompaniesCountHandler : IRequestHandler<GetTotalShippingCompaniesCountQuery, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTotalShippingCompaniesCountHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(GetTotalShippingCompaniesCountQuery request, CancellationToken cancellationToken)
        {
            var companies = await _unitOfWork.ShippingCompanyRepository.GetAllAsync();
            return companies.Count();
        }
    }
}
