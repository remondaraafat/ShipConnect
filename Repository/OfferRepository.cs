using ShipConnect.Data;
using ShipConnect.Models;
using ShipConnect.RepositoryContract;

namespace ShipConnect.Repository
{
    public class OfferRepository:GenericRepository<Offer>,IOfferRepository
    {
        public OfferRepository(ShipConnectContext context) :base(context)
        {
            
        }
    }
}
