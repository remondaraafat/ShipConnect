using System.Linq.Expressions;

namespace ShipConnect.RepositoryContract
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        IQueryable<T> GetAllAsync();
        IQueryable<T> GetWithFilterAsync(Expression<Func<T, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
        Task AddAsync(T entity);
        void Update(T entity);
        Task DeleteAsync(Expression<Func<T, bool>> predicate);

    }
}
