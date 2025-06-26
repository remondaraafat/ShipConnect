using System.Linq.Expressions;
using ShipConnect.Data;
using ShipConnect.Models;
using ShipConnect.RepositoryContract;

namespace ShipConnect.Repository
{
    public class ReceiverRepository : GenericRepository<Receiver>, IReceiverRepository
    {
        public ReceiverRepository(ShipConnectContext context) : base(context)
        {
        }
    }
}
