using ShipConnect.Data;
using ShipConnect.Models;
using ShipConnect.RepositoryContract;

namespace ShipConnect.Repository
{
    public class StartUpRepository:GenericRepository<StartUp>,IStartUpRepository
    {
        public StartUpRepository(ShipConnectContext context) : base(context)
        {
            
        }
    }
}
