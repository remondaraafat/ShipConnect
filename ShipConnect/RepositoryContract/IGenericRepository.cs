using System.Linq.Expressions;

namespace ShipConnect.RepositoryContract
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IQueryable<T>> GetAllAsync();
        Task<IQueryable<T>> GetWithFilterAsync(Expression<Func<T, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
        Task AddAsync(T entity);
        void Update(T entity);
        Task DeleteAsync(Expression<Func<T, bool>> predicate);

    }
}
