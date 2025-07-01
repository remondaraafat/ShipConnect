using ShipConnect.Data;
using ShipConnect.Models;
using ShipConnect.RepositoryContract;

namespace ShipConnect.Repository
{
    public class BankAccountRepository:GenericRepository<BankAccount>,IBankAccountRepository
    {
        public BankAccountRepository(ShipConnectContext context) : base(context)
        {
            
        }
    }
}
