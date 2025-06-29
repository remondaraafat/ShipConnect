using ShipConnect.Data;
using ShipConnect.Models;
using ShipConnect.RepositoryContract;

namespace ShipConnect.Repository
{
    public class RatingRepository:GenericRepository<Rating>,IRatingRepository
    {
        public RatingRepository(ShipConnectContext context) : base(context)
        {
            
        }
    }
}
