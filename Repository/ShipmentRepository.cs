using ShipConnect.Data;
using ShipConnect.Models;
using ShipConnect.RepositoryContract;

namespace ShipConnect.Repository
{
    public class ShipmentRepository:GenericRepository<Shipment>,IShipmentRepository
    {
        public ShipmentRepository(ShipConnectContext context) : base(context)
        {
            
        }
    }
}
