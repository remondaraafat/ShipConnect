using ShipConnect.Models;
using ShipConnect.Data;

using ShipConnect.RepositoryContract;

namespace ShipConnect.Repository
{
    public class RatingRepository : GenericRepository<Rating>, IRatingRepository
    {
        public RatingRepository(ShipConnectContext context) : base(context)
        {

        }
    }
}
