using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ShipConnect.Data;
using ShipConnect.Models;
using ShipConnect.RepositoryContract;

namespace ShipConnect.Repository
{
    public class PasswordResetCodeRepository:GenericRepository<PasswordResetCode>,IPasswordResetCodeRepository
    {
        public PasswordResetCodeRepository(ShipConnectContext shipConnectContext):base(shipConnectContext)
        {
            ShipConnectContext = shipConnectContext;
        }

        public ShipConnectContext ShipConnectContext { get; }

        public async Task<PasswordResetCode?> GetFirstOrDefaultAsync(Expression<Func<PasswordResetCode, bool>> predicate)
        {
            return await _context.Set<PasswordResetCode>().FirstOrDefaultAsync(predicate);
        }
    }
}
