using System.Linq.Expressions;
using ShipConnect.Models;

namespace ShipConnect.RepositoryContract
{
    public interface IPasswordResetCodeRepository:IGenericRepository<PasswordResetCode>
    {
        Task<PasswordResetCode?> GetFirstOrDefaultAsync(Expression<Func<PasswordResetCode, bool>> predicate);

    }
}
