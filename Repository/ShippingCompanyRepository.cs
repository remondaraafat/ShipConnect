using ShipConnect.Data;
using ShipConnect.Models;
using ShipConnect.RepositoryContract;

namespace ShipConnect.Repository
{
    public class ShippingCompanyRepository:GenericRepository<ShippingCompany>,IShippingCompanyRepository
    {
        public ShippingCompanyRepository(ShipConnectContext context) : base(context)
        {
            
        }
    }
}
